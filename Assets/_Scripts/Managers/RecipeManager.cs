using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RecipeManager : MonoBehaviour
{
    [SerializeField] private List<Recipe> twoIngRecipes = new List<Recipe>();
    [SerializeField] private List<Recipe> threeIngRecipes = new List<Recipe>();
    [SerializeField] private List<Recipe> fourIngRecipes = new List<Recipe>();
    [SerializeField] private List<Recipe> fiveIngRecipes = new List<Recipe>();

    private List<Recipe> availableRecipes = new List<Recipe>();

    private int completedRecipes = 0;

    private SpawnManager spawnManager;
    private UIManager uiManager;
    private GameManager gameManager;
    private Cauldron cauldron;

    public List<Recipe> activeRecipes;
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

        LoadNextRecipe();
    }

    public void LoadNextRecipe()
    {
        UpdateActiveRecipes();

        Recipe newRecipe = availableRecipes[Random.Range(0, availableRecipes.Count)];
        
        activeRecipes.Add(newRecipe);
        for(int i = 0; i < newRecipe.requiredIngredients.Count; i++)
        {
            ingredientModifierStruct = newRecipe.requiredIngredients[i];
            ingredientModifierStruct.isInCauldron = false;
            newRecipe.requiredIngredients[i] = ingredientModifierStruct;
        }

        spawnManager.SpawnRequiredIngredients(newRecipe.requiredIngredients);
        uiManager.CreateNewRecipeCardUI(newRecipe);
    }

    public void UpdateCurrentRecipe(Ingredient ingredient)
    {
        List<Recipe> recipesToRemove = new List<Recipe>();
        bool matchingIngredientFound = false;
        // For each active recipe
        for (int recipeIndex = 0; recipeIndex < activeRecipes.Count; recipeIndex++)
        {
            if (matchingIngredientFound)
            {
                break;
            }
            
            // For each required ingredient in the recipe
            for(int i = 0; i < activeRecipes[recipeIndex].requiredIngredients.Count; i++)
            {
                bool typeMatchesRequiredIngredient = ingredient.ingredientType == activeRecipes[recipeIndex].requiredIngredients[i].ingredient;
                bool processMatchesRequiredIngredient = ingredient.ingredientState == activeRecipes[recipeIndex].requiredIngredients[i].method;
                bool isInCauldron = activeRecipes[recipeIndex].requiredIngredients[i].isInCauldron;
                
                if(typeMatchesRequiredIngredient && processMatchesRequiredIngredient && !isInCauldron)
                {
                    ingredientModifierStruct = activeRecipes[recipeIndex].requiredIngredients[i];
                    ingredientModifierStruct.isInCauldron = true;
                    activeRecipes[recipeIndex].requiredIngredients[i] = ingredientModifierStruct;
                    matchingIngredientFound = true;
                    break;
                }
            }

            uiManager.UpdateCard(activeRecipes[recipeIndex]);

            bool isRecipeFinished = true;
            for(int i = 0; i < activeRecipes[recipeIndex].requiredIngredients.Count; i++)
            {
                if (!activeRecipes[recipeIndex].requiredIngredients[i].isInCauldron)
                {
                    isRecipeFinished = false;
                }
            }

            if (isRecipeFinished)
            {
                // Recipe is complete.
                recipesToRemove.Add(activeRecipes[recipeIndex]);
                
                uiManager.CloseRecipeCard(activeRecipes[recipeIndex]);
                completedRecipes++;
                cauldron.ResetCorruption();
                uiManager.DisplayNotificationText(true);
                LoadNextRecipe();
            }
        }

        foreach (Recipe recipeToRemove in recipesToRemove)
        {
            activeRecipes.Remove(recipeToRemove);
        }
        recipesToRemove.Clear();
    }

    public int GetNumberOfCompletedRecipes()
    {
        return completedRecipes;
    }

    void UpdateActiveRecipes()
    {
        if (completedRecipes <= 3)
        {
            availableRecipes = twoIngRecipes;
        }
        else if (completedRecipes > 3 && completedRecipes <= 6)
        {
            availableRecipes = threeIngRecipes;
        }
        else if (completedRecipes > 6 && completedRecipes <= 10)
        {
            availableRecipes = fourIngRecipes;
        }
        else
        {
            availableRecipes = fiveIngRecipes;
        }
    }
}
