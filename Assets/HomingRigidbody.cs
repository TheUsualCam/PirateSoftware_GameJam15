using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingRigidbody : MonoBehaviour
{
    public Transform target = null;

    public float speedBase = 1;

    public float despawnDistance = 1f;

    public Rigidbody rigidbody;
    
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!target)
        {
            return;
        }
        
        if (Vector3.Distance(transform.position, target.position) <= despawnDistance)
        {
            ReachedTarget();
        }
        else
        {
            rigidbody.AddForce(speedBase * Time.fixedDeltaTime * (target.position - transform.position).normalized, ForceMode.Force);
        }
    }

    protected virtual void ReachedTarget()
    {
        Debug.Log($"{name} Destroyed [Reached Cauldron]");
        Destroy(gameObject);
    }
}
