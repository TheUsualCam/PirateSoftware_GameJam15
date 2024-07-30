using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    [SerializeField] private List<Recipe> twoIngRecipes = new List<Recipe>();
    [SerializeField] private List<Recipe> threeIngRecipes = new List<Recipe>();
    [SerializeField] private List<Recipe> fourIngRecipes = new List<Recipe>();
    [SerializeField] private List<Recipe> fiveIngRecipes = new List<Recipe>();

    private List<Recipe> activeRecipes = new List<Recipe>();

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

        LoadNextRecipe();
    }

    public void LoadNextRecipe()
    {
        UpdateActiveRecipes();

        currentRecipe = activeRecipes[UnityEngine.Random.Range(0, activeRecipes.Count)];

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

    void UpdateActiveRecipes()
    {
        if (completedRecipes <= 3)
        {
            activeRecipes = twoIngRecipes;
        }
        else if (completedRecipes > 3 && completedRecipes <= 6)
        {
            activeRecipes = threeIngRecipes;
        }
        else if (completedRecipes > 6 && completedRecipes <= 10)
        {
            activeRecipes = fourIngRecipes;
        }
        else
        {
            activeRecipes = fiveIngRecipes;
        }
    }
}
