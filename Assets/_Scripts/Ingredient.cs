using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public ParticleSystem blessedParticles;
    
    public enum IngredientState
    {
        Unprepped,
        Blessed,
        Extracted,
        Dried
    }

    public string ingredientName;
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
            
            default:
                break;
        }
    }
}
