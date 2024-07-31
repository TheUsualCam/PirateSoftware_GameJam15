using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class RecipeManager : MonoBehaviour
{
    [SerializeField] private List<Recipe> twoIngRecipes = new List<Recipe>();
    public float availableAtCompletedRecipes_TwoIngredients = 0;
    [SerializeField] private List<Recipe> threeIngRecipes = new List<Recipe>();
    public float availableAtCompletedRecipes_ThreeIngredients = 2;
    [SerializeField] private List<Recipe> fourIngRecipes = new List<Recipe>();
    public float availableAtCompletedRecipes_FourIngredients = 6;
    [SerializeField] private List<Recipe> fiveIngRecipes = new List<Recipe>();
    public float availableAtCompletedRecipes_FiveIngredients = 8;

    public List<float> timesToSpawnNewRecipe = new List<float>();
    public float chanceToReturnRecipeAfterFinishing = 0.5f;
    

    public List<Recipe> availableRecipes = new List<Recipe>();

    private int completedRecipes = 0;

    private SpawnManager spawnManager;
    private UIManager uiManager;
    private GameManager gameManager;
    private Cauldron cauldron;

    public List<Recipe> activeRecipes;
    private RequiredIngredient ingredientModifierStruct;

    public AudioClip correctIngredientClip;
    public AudioClip incorrectIngredientClip;
    public AudioClip completeRecipeClip;
    

    private void OnEnable()
    {
        Cauldron.OnCauldronCorrupted += LoadAdditionalRecipe;
    }

    private void OnDisable()
    {
        Cauldron.OnCauldronCorrupted -= LoadAdditionalRecipe;

    }

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        uiManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();
        cauldron = FindObjectOfType<Cauldron>();

        LoadAdditionalRecipe();

        StartCoroutine(ELoadRecipesForGame());
    }

    private IEnumerator ELoadRecipesForGame()
    {
        
        for (int i = 0; i < timesToSpawnNewRecipe.Count; i++)
        {
            yield return new WaitForSeconds(Mathf.Max(0f, timesToSpawnNewRecipe[i] - Time.timeSinceLevelLoad));
            LoadAdditionalRecipe();
            timesToSpawnNewRecipe.RemoveAt(i);
            i--;
        }
        
        yield return null;
    }

    public void LoadAdditionalRecipe()
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

    public void IngredientAddedToCauldron(Ingredient ingredient)
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
                    // Ingredient matches, mark is as in cauldron.
                    ingredientModifierStruct = activeRecipes[recipeIndex].requiredIngredients[i];
                    ingredientModifierStruct.isInCauldron = true;
                    activeRecipes[recipeIndex].requiredIngredients[i] = ingredientModifierStruct;
                    matchingIngredientFound = true;
                    AudioManager.instance.PlaySoundClip(correctIngredientClip, this.transform, 1f);
                    break;
                }
            }
            
            if (matchingIngredientFound)
            {
                uiManager.UpdateCard(activeRecipes[recipeIndex]);

                // Check if the recipe is finished.
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
                    AudioManager.instance.PlaySoundClip(completeRecipeClip, this.transform, 1f);
                    
                    uiManager.CloseRecipeCard(activeRecipes[recipeIndex]);
                    completedRecipes++;
                    cauldron.ResetCorruption();
                    uiManager.DisplayNotificationText(true);
                    if (Random.Range(0f, 1f) <= chanceToReturnRecipeAfterFinishing)
                    {
                        LoadAdditionalRecipe();
                    }
                }
            }

        }

        if (!matchingIngredientFound)
        {
            spawnManager.RespawnIngredient(ingredient);
            AudioManager.instance.PlaySoundClip(incorrectIngredientClip, this.transform, 1f);
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
        if (completedRecipes > availableAtCompletedRecipes_FiveIngredients)
        {
            availableRecipes = twoIngRecipes;
        }
        else if (completedRecipes > availableAtCompletedRecipes_FourIngredients && completedRecipes <= availableAtCompletedRecipes_FiveIngredients)
        {
            availableRecipes = fourIngRecipes;
        }
        else if (completedRecipes > availableAtCompletedRecipes_ThreeIngredients && completedRecipes <= availableAtCompletedRecipes_FourIngredients)
        {
            availableRecipes = threeIngRecipes;
        }
        else
        {
            availableRecipes = twoIngRecipes;
        }
    }

    public void RemoveCorruptedRecipe()
    {
        uiManager.CloseRecipeCard(activeRecipes[0]);
        activeRecipes.RemoveAt(0);
    }
}
