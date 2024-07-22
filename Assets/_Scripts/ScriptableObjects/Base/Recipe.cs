using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRecipe", menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    public enum PrepMethod
    {
        Extracted,
        Dried,
        Blessed
    }

    [System.Serializable]
    public struct RequiredIngredient
    {
        public Ingredient ingredient;
        public PrepMethod method;
    }

    public List<RequiredIngredient> requiredIngredients = new List<RequiredIngredient>();
}
