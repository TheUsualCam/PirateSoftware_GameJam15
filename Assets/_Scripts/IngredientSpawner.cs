using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnableIngredients;
    [SerializeField] private Transform spawnPosition;

    public void SpawnRequiredIngredients(List<RequiredIngredient> requiredIngredients)
    {
        for(int i = 0; i < requiredIngredients.Count; i++)
        {
            Instantiate(requiredIngredients[i].ingredient, spawnPosition.position, Quaternion.identity);
        }
    }
}
