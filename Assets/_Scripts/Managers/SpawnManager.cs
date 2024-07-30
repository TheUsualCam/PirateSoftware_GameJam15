using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Ingredient Spawner")]
    [SerializeField] private Transform ingredientSpawnPosition;
    
    private List<Ingredient> spawnedIngredients = new List<Ingredient>();
    private Ingredient newIngredient;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
    }

    public void SpawnRequiredIngredients(List<RequiredIngredient> requiredIngredients)
    {
        /*
        for (int i = spawnedIngredients.Count - 1; i > 0; i--)
        {
            if (spawnedIngredients[i])
            {
                Destroy(spawnedIngredients[i].gameObject);
            }
        }

        spawnedIngredients.Clear();
        */

        for (int i = 0; i < requiredIngredients.Count; i++)
        {
            newIngredient = Instantiate(requiredIngredients[i].ingredient, ingredientSpawnPosition.position, Quaternion.identity);
            spawnedIngredients.Add(newIngredient);
        }
    }

    public void RespawnIngredient(Ingredient ingredient)
    {
        newIngredient = Instantiate(ingredient, ingredientSpawnPosition.position, Quaternion.identity);
        spawnedIngredients.Add(newIngredient);
    }
}
