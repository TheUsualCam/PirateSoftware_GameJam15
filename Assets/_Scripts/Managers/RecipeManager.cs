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

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        uiManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();
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
            currentRecipe = recipes[recipeIndex];
            recipeIndex++;
            spawnManager.SpawnRequiredIngredients(currentRecipe.requiredIngredients);
            spawnManager.SpawnShadows();
            uiManager.UpdateRecipeUI(currentRecipe);
        }
    }

    public Recipe GetCurrentRecipe()
    {
        return currentRecipe;
    }
}
