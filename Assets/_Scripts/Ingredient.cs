using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public enum IngredientState
    {
        Unprepped,
        Blessed,
        Extracted,
        Dried
    }

    public string ingredientName;
    public IngredientState ingredientState;
}
