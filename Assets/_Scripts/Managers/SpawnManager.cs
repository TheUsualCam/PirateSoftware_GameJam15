using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Ingredient Spawner")]
    [SerializeField] private Transform ingredientSpawnPosition;

    [Space, Header("Shadow Spawner")]
    [SerializeField] private GameObject shadowPrefab;
    [SerializeField] private Transform cauldronPosition;
    [SerializeField] private float minSpawnRadius = 3.0f;
    [SerializeField] private float maxSpawnRadius = 10.0f;
    [SerializeField] float shadowSpawnHeight = 5.0f;
    [SerializeField] int shadowsPerSpawn = 3;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnRequiredIngredients(List<RequiredIngredient> requiredIngredients)
    {
        for (int i = 0; i < requiredIngredients.Count; i++)
        {
            Instantiate(requiredIngredients[i].ingredient, ingredientSpawnPosition.position, Quaternion.identity);
        }
    }

    public void SpawnShadows()
    {
        for (int i = 0; i < shadowsPerSpawn; i++)
        {
            Vector3 shadowSpawnPosition = GetShadowSpawnPosition();
            shadowSpawnPosition.y = shadowSpawnHeight;
            
            Instantiate(shadowPrefab, shadowSpawnPosition, Quaternion.identity);
        }
    }

    private Vector3 GetShadowSpawnPosition()
    {
        float theta = Random.Range(0, 2 * Mathf.PI);
        float randRadius = Mathf.Lerp(minSpawnRadius / 2f, maxSpawnRadius / 2f, Random.Range(0, 1f));
        
        Vector3 pos = new Vector3(randRadius * Mathf.Cos(theta),0,randRadius * Mathf.Sin(theta));
        return cauldronPosition.position + pos;
    }
}
