using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronTrigger : MonoBehaviour
{
    private RecipeManager recipeManager;

    private void Start()
    {
        recipeManager = FindObjectOfType<RecipeManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Ingredient>())
        {
            recipeManager.IngredientAddedToCauldron(other.GetComponent<Ingredient>());
            Debug.Log("Ingredient dropped in cauldron!");
            Destroy(other.gameObject);
        }
    }
}
