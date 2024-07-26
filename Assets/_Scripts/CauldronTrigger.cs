using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Ingredient>())
        {
            Debug.Log("Ingredient dropped in cauldron!");
            Destroy(other.gameObject);
        }
    }
}
