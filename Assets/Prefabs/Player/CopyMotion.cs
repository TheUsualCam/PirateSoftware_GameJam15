using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMotion : MonoBehaviour
{
    public Transform targetLimb;

    private ConfigurableJoint _configurableJoint;

    private Quaternion initialLocalRotation;
    // Start is called before the first frame update
    void Start()
    {
        _configurableJoint = GetComponent<ConfigurableJoint>();

        initialLocalRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        _configurableJoint.SetTargetRotationLocal(targetLimb.localRotation, initialLocalRotation);
    }
}
