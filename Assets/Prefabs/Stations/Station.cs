using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Station : MonoBehaviour
{
    private float updatesPerSecond = 5;
    private float nextUpdateTime;
    public struct ProcessingIngredient
    {
        public Ingredient _ingredient;
        public float finishTime;
        public Rigidbody _rigidbody;

        public ProcessingIngredient(Ingredient ingredient, float time, Rigidbody rigidbody)
        {
            _ingredient = ingredient;
            finishTime = time;
            _rigidbody = rigidbody;
        }
    }
    
    [Tooltip("How base time (seconds) it takes for this station to complete.")]
    [SerializeField] private float baseDuration = 0.5f;
    [SerializeField] private Slider stationSlider = null;
    [SerializeField] private Image stationSliderFill = null;

    public List<ProcessingIngredient> heldIngredients = new List<ProcessingIngredient>();
    
    public Ingredient.IngredientState targetState;
    public ParticleSystem[] processingParticles;
    
    [Header("Finished Ingredients")]
    public Transform finishedIngredientSpawnPoint;
    public Vector2 throwStrengthRange;
    public Vector3 throwDirection;
    public Vector2 throwXRange;

    public static event Action<Ingredient> OnIngredientProcessingStarted;

    [Header("Audio")] 
    public AudioClip processingAudio;
    private AudioSource processingAudioSource;
    public AudioClip releaseClip;

    private void Awake()
    {
        processingAudioSource = GetComponent<AudioSource>();
        processingAudioSource.clip = processingAudio;
    }

    private void OnTriggerEnter(Collider enteredCollider)
    {
        Ingredient ingredient = enteredCollider.gameObject.GetComponent<Ingredient>();

        if (ingredient && ingredient.ingredientState == Ingredient.IngredientState.Unprepped)
        {
            NewIngredient(ingredient);
        }
    }

    void NewIngredient(Ingredient ingredient)
    {
        // Cache the item
        ProcessingIngredient newItem = new ProcessingIngredient(ingredient, Time.time + baseDuration, ingredient.GetComponent<Rigidbody>());
        heldIngredients.Add(newItem);
        stationSliderFill.color = Ingredient.GetIngredientColour(ingredient.ingredientType);
        ShowUiSlider();

        OnIngredientProcessingStarted?.Invoke(ingredient);
        
        // Hide and reset the physics of the object
        newItem._rigidbody.velocity = Vector3.zero;
        ingredient.gameObject.SetActive(false);
        ingredient.transform.SetParent(transform);
        ingredient.transform.localPosition = Vector3.zero;
    }

    private void ShowUiSlider()
    {
        stationSlider.gameObject.SetActive(true);
        stationSlider.value = 0;
    }

    void Update()
    {
        if (Time.time > nextUpdateTime)
        {
            nextUpdateTime = Time.time + 1f / updatesPerSecond;
            
            CheckForReadyIngredients();
            ProcessParticleSystems();
            
            if(heldIngredients.Count > 0)
            {
                float timeRemaining = heldIngredients[0].finishTime - Time.time;
                
                stationSlider.value = 1 - (timeRemaining / baseDuration);

                if (!processingAudioSource.isPlaying)
                {
                    processingAudioSource.Play();
                }
            }
            else
            {
                HideUiSlider();
                if (processingAudioSource.isPlaying)
                {
                    processingAudioSource.Stop();
                }
            }
            
        }
    }

    private void HideUiSlider()
    {
        if (stationSlider.gameObject.activeSelf)
        {
            stationSlider.gameObject.SetActive(false);
            stationSlider.value = 0.0f;
        }
            
    }

    private void CheckForReadyIngredients()
    {
        List<ProcessingIngredient> IngredientsToRemove = new List<ProcessingIngredient>();

        foreach (ProcessingIngredient ingredient in heldIngredients)
        {
            if (ingredient._ingredient.transform == null)
            {
                IngredientsToRemove.Add(ingredient);
                continue;
            }
            if (Time.time >= ingredient.finishTime)
            {
                ReleaseIngredient(ingredient);
                IngredientsToRemove.Add(ingredient);
            }
        }

        foreach (ProcessingIngredient ingredient in IngredientsToRemove)
        {
            heldIngredients.Remove(ingredient);
        }

        IngredientsToRemove.Clear();
    }

    private void ProcessParticleSystems()
    {
        if (processingParticles.Length > 0)
        {
            if (heldIngredients.Count > 0)
            {
                if (!processingParticles[0].isPlaying)
                {
                    foreach (ParticleSystem particleSystem in processingParticles)
                    {
                        particleSystem.Play();
                    }
                }
            }
            else if (processingParticles[0].isPlaying)
            {
                foreach (ParticleSystem particleSystem in processingParticles)
                {
                    particleSystem.Stop();
                }
            }
        }
    }

    void ReleaseIngredient(ProcessingIngredient processingIngredient)
    {
        Ingredient ingredient = processingIngredient._ingredient;
        ingredient.transform.SetParent(null);
        ingredient.GetComponent<MeshCollider>().enabled = false;
        StartCoroutine(ReEnableMeshCollider(ingredient));
        ingredient.gameObject.SetActive(true);
        ingredient.ChangeState(targetState);
        
        ingredient.transform.SetLocalPositionAndRotation(finishedIngredientSpawnPoint.position, finishedIngredientSpawnPoint.rotation);

        throwDirection.x = Random.Range(throwXRange.x, throwXRange.y);
        processingIngredient._rigidbody.AddForce(finishedIngredientSpawnPoint.TransformDirection(throwDirection) * Random.Range(throwStrengthRange.x, throwStrengthRange.y), ForceMode.Impulse);
        stationSlider.value = 0;
        stationSlider.gameObject.SetActive(false);
        AudioManager.instance.PlaySoundClip(releaseClip, ingredient.transform, 1f);
    }

    IEnumerator ReEnableMeshCollider(Ingredient ingredient)
    {
        yield return new WaitForSeconds(0.2f);
        ingredient.GetComponent<MeshCollider>().enabled = true;
    }
}
