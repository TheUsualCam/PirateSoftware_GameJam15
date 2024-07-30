using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CauldronSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float spawnImpulseForce = 3.0f;
    [SerializeField] private Vector3 spawnImpulsePower;
    [SerializeField] int numberPerSpawn = 3;
    [SerializeField] float delayBetweenSpawns = 0.1f;


    private void OnEnable()
    {
        Cauldron.OnCauldronCorrupted += Spawn;
    }

    private void OnDisable()
    {
        Cauldron.OnCauldronCorrupted -= Spawn;
    }

    private void Spawn()
    {
        StartCoroutine(ESpawn());
    }

    private IEnumerator ESpawn()
    {
        for (int i = 0; i < numberPerSpawn; i++)
        {
            // Create and set target to cauldron
            Rigidbody newObject = Instantiate(spawnPrefab, spawnPos.position, spawnPos.rotation).GetComponent<Rigidbody>();
            
            newObject.AddForce(spawnImpulseForce * GetImpulseDirection(), ForceMode.Impulse);
            ShadowCreature shadow = newObject.GetComponent<ShadowCreature>();
            if (shadow)
            {
                shadow.target = transform;
            }
            
            yield return new WaitForSeconds(delayBetweenSpawns);
        }

        yield return null;
    }

    private Vector3 GetImpulseDirection()
    {
        float theta = Random.Range(0, 2 * Mathf.PI);
        
        return new Vector3(Mathf.Cos(theta) * spawnImpulsePower.x,spawnImpulsePower.y,Mathf.Sin(theta) * spawnImpulsePower.z);
    }
}
