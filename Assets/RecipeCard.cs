using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeCard : MonoBehaviour
{
    [Header("Ui Sprites\nThese Lists should follow the below order:\nUnprepped, Blessed, Extracted, Dried.")]
    public List<Sprite> MeatSprites;
    public List<Sprite> PlantSprites;
    public List<Sprite> MineralSprites;
    public List<Sprite> MagicSprites;
    
    public Animator animator;
    public Recipe recipe;
    public List<Image> icons;
    public List<GameObject> iconTicks = new List<GameObject>();

    public void Initialize(Recipe newRecipe)
    {
        recipe = newRecipe;
        for (int i = 0; i < recipe.requiredIngredients.Count; i++)
        {
            if (i >= icons.Count)
            {
                Debug.LogError($"Recipe: {recipe.name} has too many ingredients. Cannot display on recipe card.");
                return;
            }

            icons[i].sprite = GetUiSprite(recipe.requiredIngredients[i].ingredient, recipe.requiredIngredients[i].method);
            icons[i].gameObject.SetActive(true);
        }

        for (int i = recipe.requiredIngredients.Count; i < icons.Count; i++)
        {
            icons[i].gameObject.SetActive(false);
            icons[i].sprite = null;
        }
    }

    private Sprite GetUiSprite(Ingredient.IngredientType ingredient, Ingredient.IngredientState method)
    {
        switch (ingredient)
        {
            case Ingredient.IngredientType.Meat:
                return MeatSprites[(int)method];
            case Ingredient.IngredientType.Plant:
                return PlantSprites[(int)method];
            case Ingredient.IngredientType.Mineral:
                return MineralSprites[(int)method];
            case Ingredient.IngredientType.MagicItem:
                return MagicSprites[(int)method];
        }

        return null;
    
    }

    public void UpdateCard(Recipe updatedRecipe)
    {
        for(int i = 0; i < updatedRecipe.requiredIngredients.Count; i++)
        {
            if (i >= iconTicks.Count)
            {
                Debug.LogError($"Updating Recipe: {recipe.name} has too many ingredients. Cannot display Ticks on recipe card.");
                return;
            }
            
            if (updatedRecipe.requiredIngredients[i].isInCauldron)
            {
                iconTicks[i].SetActive(true);
            }
            else
            {
                iconTicks[i].SetActive(false);
            }
            
            
        }
    }

    public void CloseCard()
    {
        animator.SetTrigger("Close");
    }

    public void CardClosed()
    {
        Destroy(gameObject);
    }
}
