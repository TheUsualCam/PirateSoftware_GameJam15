using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCreature : HomingRigidbody
{
    public float floatingHeight;
    public float delayBeforeMoving = 5f;
    private float moveAfterTime;

    void Start()
    {
        moveAfterTime = Time.time + delayBeforeMoving;
    }
    
    protected override void ReachedTarget()
    {
        Cauldron cauldron = target.GetComponent<Cauldron>();
        if (cauldron)
        {
            cauldron.ShadowEntered();
            Destroy(gameObject);
        }
    }

    protected override void FixedUpdate()
    {
        if (Time.time < moveAfterTime)
        {
            return;
        }
        
        base.FixedUpdate();
        if (transform.position.y <= floatingHeight)
        {
            rigidbody.useGravity = false;
            rigidbody.velocity = Vector3.zero;
            homingEnabled = true;
        }
    }

}
