using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingRigidbody : MonoBehaviour
{
    protected bool homingEnabled = false;
    
    public Transform target = null;

    public float speedBase = 1;

    public float despawnDistance = 1f;

    public Rigidbody rigidbody;
    
    
    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (!target || !homingEnabled)
        {
            return;
        }
        
        if (Vector3.Distance(transform.position, target.position) <= despawnDistance)
        {
            ReachedTarget();
        }
        else
        {
            Vector3 directionToTarget = target.position - transform.position;
            directionToTarget.y = 0f;
            rigidbody.AddForce(speedBase * Time.fixedDeltaTime * directionToTarget.normalized, ForceMode.Force);
        }
    }

    protected virtual void ReachedTarget()
    {
        Debug.Log($"{name} Destroyed [Reached Cauldron]");
        Destroy(gameObject);
    }
}
