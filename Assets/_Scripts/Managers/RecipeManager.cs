using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    [SerializeField] private List<Recipe> recipes = new List<Recipe>();
    private int completedRecipes = 0;

    private SpawnManager spawnManager;
    private UIManager uiManager;
    private GameManager gameManager;
    private Cauldron cauldron;

    private Recipe currentRecipe;
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
        cauldron = FindObjectOfType<Cauldron>();

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
        currentRecipe = recipes[UnityEngine.Random.Range(0, recipes.Count)];

        for(int i = 0; i < currentRecipe.requiredIngredients.Count; i++)
        {
            ingredientModifierStruct = currentRecipe.requiredIngredients[i];
            ingredientModifierStruct.isInCauldron = false;
            currentRecipe.requiredIngredients[i] = ingredientModifierStruct;
        }

        spawnManager.SpawnRequiredIngredients(currentRecipe.requiredIngredients);
        uiManager.UpdateRecipeUI(currentRecipe);
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
            else if(i == currentRecipe.requiredIngredients.Count - 1)
            {
                cauldron.AddCorruption(cauldron.GetCorruptionPerBadIngredient());
                spawnManager.RespawnIngredient(ingredient);
            }
        }

        uiManager.UpdateRecipeUI(currentRecipe);

        for(int i = 0; i < currentRecipe.requiredIngredients.Count; i++)
        {
            if (!currentRecipe.requiredIngredients[i].isInCauldron)
            {
                return;
            }
        }

        completedRecipes++;
        cauldron.ResetCorruption();
        uiManager.DisplayNotificationText(true);
        LoadNextRecipe();
    }

    public Recipe GetCurrentRecipe()
    {
        return currentRecipe;
    }

    public int GetNumberOfCompletedRecipes()
    {
        return completedRecipes;
    }
}
