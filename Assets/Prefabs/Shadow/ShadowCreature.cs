using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCreature : HomingRigidbody
{
    public float floatingHeight;
    
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
        base.FixedUpdate();
        if (transform.position.y <= floatingHeight)
        {
            rigidbody.useGravity = false;
            rigidbody.velocity = Vector3.zero;
            homingEnabled = true;
        }
    }

}
