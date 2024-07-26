using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCreature : HomingRigidbody
{
    protected override void ReachedTarget()
    {
        Cauldron cauldron = target.GetComponent<Cauldron>();
        if (cauldron)
        {
            cauldron.ShadowEntered();
            Destroy(gameObject);
        }
    }
}
