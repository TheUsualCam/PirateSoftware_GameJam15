using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] ingredientsToSpawn;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private float spawnInterval = 3.0f;

    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < spawnInterval)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Instantiate(ingredientsToSpawn[Random.Range(0, ingredientsToSpawn.Length)], spawnPosition.position, Quaternion.identity);
            timer = 0;
        }
    }
}
