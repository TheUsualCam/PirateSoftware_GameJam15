using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("List = Meat, Plant, Mineral, Magic")]
    public GameObject[] ingredientPrefabs;
    [Header("Ingredient Spawner")]
    [SerializeField] private Transform ingredientSpawnPosition;
    
    private List<Ingredient> spawnedIngredients = new List<Ingredient>();
    private Ingredient newIngredient;
    public float initialSpawnDelay = 0.3f;
    public float delayBetweenSpawn = 0.1f;
    public AudioClip spawnStartSound;
    public AudioClip spawnEndSound;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
    }

    public void SpawnRequiredIngredients(List<RequiredIngredient> requiredIngredients)
    {
        StartCoroutine(ESpawnRequiredIngredients(requiredIngredients));
    }

    private IEnumerator ESpawnRequiredIngredients(List<RequiredIngredient> requiredIngredients)
    {
        AudioManager.instance.PlaySoundClip(spawnStartSound, ingredientSpawnPosition, 1f);
        
        yield return new WaitForSeconds(initialSpawnDelay);
        
        for (int i = 0; i < requiredIngredients.Count; i++)
        {
            newIngredient = Instantiate(ingredientPrefabs[(int)requiredIngredients[i].ingredient], ingredientSpawnPosition.position, Quaternion.identity).GetComponent<Ingredient>();
            spawnedIngredients.Add(newIngredient);
            yield return new WaitForSeconds(delayBetweenSpawn);
        }
        
        yield return new WaitForSeconds(initialSpawnDelay);

        AudioManager.instance.PlaySoundClip(spawnEndSound, ingredientSpawnPosition, 1f);

    }


    public void RespawnIngredient(Ingredient ingredient)
    {
        newIngredient = Instantiate(ingredientPrefabs[(int)ingredient.ingredientType], ingredientSpawnPosition.position, Quaternion.identity).GetComponent<Ingredient>();
        spawnedIngredients.Add(newIngredient);
    }
}
