using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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

    public List<ProcessingIngredient> heldIngredients = new List<ProcessingIngredient>();
    
    public Ingredient.IngredientState targetState;
    public ParticleSystem[] processingParticles;
    
    [Header("Finished Ingredients")]
    public Transform finishedIngredientSpawnPoint;
    public Vector2 throwStrengthRange;
    public Vector3 throwDirection;
    public Vector2 throwXRange;

    public static event Action<Ingredient> OnIngredientProcessingStarted;

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
        
        OnIngredientProcessingStarted?.Invoke(ingredient);
        
        // Hide and reset the physics of the object
        newItem._rigidbody.velocity = Vector3.zero;
        ingredient.gameObject.SetActive(false);
        ingredient.transform.SetParent(transform);
        ingredient.transform.localPosition = Vector3.zero;
    }

    void Update()
    {
        if (Time.time > nextUpdateTime)
        {
            nextUpdateTime = Time.time + 1f / updatesPerSecond;
            CheckForReadyIngredients();
            ProcessParticleSystems();
        }
    }

    private void CheckForReadyIngredients()
    {
        List<ProcessingIngredient> IngredientsToRelease = new List<ProcessingIngredient>();

        foreach (ProcessingIngredient ingredient in heldIngredients)
        {
            if (Time.time >= ingredient.finishTime)
            {
                ReleaseIngredient(ingredient);
                IngredientsToRelease.Add(ingredient);
            }
        }

        foreach (ProcessingIngredient ingredient in IngredientsToRelease)
        {
            heldIngredients.Remove(ingredient);
        }

        IngredientsToRelease.Clear();
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
        ingredient.gameObject.SetActive(true);
        ingredient.ChangeState(targetState);
        
        ingredient.transform.SetLocalPositionAndRotation(finishedIngredientSpawnPoint.position, finishedIngredientSpawnPoint.rotation);

        throwDirection.x = Random.Range(throwXRange.x, throwXRange.y);
        processingIngredient._rigidbody.AddForce(finishedIngredientSpawnPoint.TransformDirection(throwDirection) * Random.Range(throwStrengthRange.x, throwStrengthRange.y), ForceMode.Impulse);
    }
    
    
}
