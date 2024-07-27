using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    [SerializeField] private List<Recipe> recipes = new List<Recipe>();

    private SpawnManager spawnManager;
    private UIManager uiManager;
    private GameManager gameManager;

    private Recipe currentRecipe;
    private int recipeIndex = 0;
    private RequiredIngredient ingredientModifierStruct;

    private void OnEnable()
    {
        Cauldron.OnCauldronCorrupted += LoadNextRecipe;
    }

    private void OnDisable()
    {
        Cauldron.OnCauldronCorrupted -= LoadNextRecipe;

    }

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        uiManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();

        for(int i = 0; i < recipes.Count; i++)
        {
            for(int j = 0; j < recipes[i].requiredIngredients.Count; j++)
            {
                ingredientModifierStruct = recipes[i].requiredIngredients[j];
                ingredientModifierStruct.isInCauldron = false;
                recipes[i].requiredIngredients[j] = ingredientModifierStruct;
            }
        }

        LoadNextRecipe();
    }

    public void LoadNextRecipe()
    {
        if(recipeIndex >= recipes.Count)
        {
            Debug.Log("No more recipes!");
            gameManager.GameOver();
        }
        else
        {
            Debug.Log($"Loading Recipe...");
            currentRecipe = recipes[recipeIndex];
            recipeIndex++;
            spawnManager.SpawnRequiredIngredients(currentRecipe.requiredIngredients);
            spawnManager.SpawnShadows();
            uiManager.UpdateRecipeUI(currentRecipe);
        }
    }

    public void UpdateCurrentRecipe(Ingredient ingredient)
    {
        for(int i = 0; i < currentRecipe.requiredIngredients.Count; i++)
        { 
            ingredientModifierStruct = currentRecipe.requiredIngredients[i];

            if(ingredient.ingredientType == currentRecipe.requiredIngredients[i].ingredient.ingredientType && ingredient.ingredientState.ToString() == currentRecipe.requiredIngredients[i].method.ToString())
            {
                ingredientModifierStruct.isInCauldron = true;
                currentRecipe.requiredIngredients[i] = ingredientModifierStruct;
                break;
            }
        }

        uiManager.UpdateRecipeUI(currentRecipe);
    }

    public Recipe GetCurrentRecipe()
    {
        return currentRecipe;
    }
}
