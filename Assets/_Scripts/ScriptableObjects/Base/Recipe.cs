using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Recipe;

[CreateAssetMenu(fileName = "newRecipe", menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    public enum PrepMethod
    {
        Extracted,
        Dried,
        Blessed
    }

    public List<RequiredIngredient> requiredIngredients = new List<RequiredIngredient>();
}

[System.Serializable]
public struct RequiredIngredient
{
    public Ingredient ingredient;
    public PrepMethod method;
    public bool isInCauldron;
}