using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    [SerializeField] private List<Recipe> recipes = new List<Recipe>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < recipes.Count; i++)
        {
            for(int j = 0; j < recipes[i].requiredIngredients.ingredients.Count; j++)
            {
                Debug.Log(recipes[i].requiredIngredients.ingredients[j].ingredientName + ", " + recipes[i].requiredIngredients.methods[j]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
