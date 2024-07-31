using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Recipe;

[CreateAssetMenu(fileName = "newRecipe", menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    public List<RequiredIngredient> requiredIngredients = new List<RequiredIngredient>();
}

[System.Serializable]
public struct RequiredIngredient
{
    public Ingredient.IngredientType ingredient;
    public Ingredient.IngredientState method;
    public bool isInCauldron;
}