using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachBetweenTransforms : MonoBehaviour
{

    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform targetA;
    [SerializeField] private Transform targetB;

    // Update is called once per frame
    void Update()
    {
        transform.position = offset + (targetA.position + targetB.position) / 2;
    }
}
