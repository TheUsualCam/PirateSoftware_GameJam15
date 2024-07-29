using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public ParticleSystem blessedParticles;
    public ParticleSystem driedParticles;
    public ParticleSystem ExtractedParticles;
    
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

    public void ChangeState(IngredientState newState)
    {
        if (Equals(newState, ingredientState))
        {
            return;
        }
        
        ingredientState = newState;

        switch (ingredientState)
        {
            case IngredientState.Blessed:
                blessedParticles.Play();
                break;
            case IngredientState.Dried:
                driedParticles.Play();
                break;
            case IngredientState.Extracted:
                ExtractedParticles.Play();
                break;
            default:
                break;
        }
    }
}
