using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public AudioClip spawnSound;
    [Header("Blessed State")]
    public ParticleSystem blessedParticles;

    [Space, Header("Extracted State")]
    public Mesh extractedMesh;
    public Material[] extractedMaterials;
    public ParticleSystem ExtractedParticles;

    [Space, Header("Dried State")]
    public Mesh driedMesh;
    public Material[] driedMaterials;
    public ParticleSystem driedParticles;

    public enum IngredientType
    {
        Meat,
        Plant,
        MagicItem,
        Mineral
    }

    public enum IngredientState
    {
        Unprepped,
        Blessed,
        Extracted,
        Dried
    }

    public string ingredientName;
    public IngredientType ingredientType;
    public IngredientState ingredientState;

    void Start()
    {
        AudioManager.instance.PlaySoundClip(spawnSound, transform, 1f);
    }
    void FixedUpdate()
    {
        if (transform.position.y < -1f)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, 2.5f, transform.position.z);
        }
    }
    public void ChangeState(IngredientState newState)
    {
        if (Equals(newState, ingredientState))
        {
            return;
        }
        
        ingredientState = newState;
        Mesh currentMesh = GetComponent<MeshFilter>().mesh;
        Material[] currentMaterials = GetComponent<MeshRenderer>().materials;

        switch (ingredientState)
        {
            case IngredientState.Blessed:
                blessedParticles.Play();
                break;
            case IngredientState.Dried:
                currentMesh = driedMesh;
                currentMaterials = driedMaterials;
                driedParticles.Play();
                break;
            case IngredientState.Extracted:
                currentMesh = extractedMesh;
                currentMaterials = extractedMaterials;
                ExtractedParticles.Play();
                break;
            default:
                break;
        }

        GetComponent<MeshFilter>().mesh = currentMesh;
        GetComponent<MeshRenderer>().materials = currentMaterials;
        GetComponent<MeshCollider>().sharedMesh = currentMesh;
        GetComponent<MeshCollider>().sharedMesh = currentMesh;
        if (ingredientState != IngredientState.Blessed)
        {
            transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }
    }
}
