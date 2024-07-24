using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    [SerializeField] private List<Recipe> recipes = new List<Recipe>();

    private IngredientSpawner ingredientSpawner;
    private UIManager uiManager;
    private Recipe currentRecipe;
    private int recipeIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        ingredientSpawner = FindObjectOfType<IngredientSpawner>();
        uiManager = FindObjectOfType<UIManager>();
        LoadNextRecipe();
    }

    public void LoadNextRecipe()
    {
        if(recipes.Count == 0)
        {
            return;
        }

        currentRecipe = recipes[recipeIndex];
        recipeIndex++;
        ingredientSpawner.SpawnRequiredIngredients(currentRecipe.requiredIngredients);
        uiManager.UpdateRecipeUI(currentRecipe);
    }
}
