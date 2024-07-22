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
    public struct RequiredIngredients
    {
        public List<Ingredient> ingredients;
        public List<PrepMethod> methods;
    }

    public RequiredIngredients requiredIngredients;
}
