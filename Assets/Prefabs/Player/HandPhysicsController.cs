using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class HandPhysicsController : MonoBehaviour
{
    public bool _isGrabActive = false;
    [SerializeField] private Transform _balancer;
    
    [Header("Searching")]
    [SerializeField] private Transform _grabSearchOrigin;
    [SerializeField] private float grabRadius = 0.8f;
    
    [Header("Hand Forces")]
    [SerializeField] private Rigidbody leftHandRigidbody;
    [SerializeField] private Rigidbody rightHandRigidBody;
    [SerializeField] private float grabForce = 10000f;
    [SerializeField] private Vector3 grabVector;
    [SerializeField] private float handsForwardsForce = 10000f;

    [Header("Holding Object")]
    [SerializeField] private Transform grabbedObject;
    [SerializeField] private Rigidbody holdOrigin;
    [SerializeField] private float dropForce = 10000f;


    private void OnEnable()
    {
        GameInput.onLeftActionStart += StartGrab;
        GameInput.onLeftActionEnd += EndGrab;
        
    }

    private void OnDisable()
    {
        GameInput.onLeftActionStart -= StartGrab;
        GameInput.onLeftActionEnd -= EndGrab;
    }

    private void FixedUpdate()
    {
        if (_isGrabActive)
        {
            AddForceToHands(grabForce, grabVector, ForceMode.Force);
        }
        else
        {
            AddForceToHands(handsForwardsForce, Vector3.forward, ForceMode.Force);
        }
    }

    private void AddForceToHands(float force, Vector3 direction, ForceMode forceMode)
    {
        leftHandRigidbody.AddForce(force * Time.fixedDeltaTime * _balancer.TransformDirection(direction).normalized, forceMode);
        Vector3 rightHandDirection = direction;
        rightHandDirection.x *= -1f;
        rightHandRigidBody.AddForce(force * Time.fixedDeltaTime * _balancer.TransformDirection(rightHandDirection).normalized, forceMode);
    }

    void StartGrab()
    {
        _isGrabActive = true;

        Collider[] overlaps = Physics.OverlapSphere(_grabSearchOrigin.position, grabRadius);
        foreach(Collider overlap in overlaps)
        {
            if (overlap.transform.CompareTag("Grabable"))
            {
                grabbedObject = overlap.transform;
                //grabbedJoint = overlap.gameObject.AddComponent<ConfigurableJoint>();
                grabbedObject.SetParent(holdOrigin.transform);
                grabbedObject.localPosition = Vector3.zero;
                grabbedObject.localRotation = Quaternion.identity;
                overlap.attachedRigidbody.isKinematic = true;
                
                return;

            }
        }
        
    }
    
    void EndGrab()
    {
        _isGrabActive = false;
        if (grabbedObject)
        {
            grabbedObject.SetParent(null, true);
            Rigidbody grabbedRB = grabbedObject.gameObject.GetComponent<Rigidbody>();
            grabbedRB.isKinematic = false;
            grabbedRB.AddForce(dropForce * _balancer.TransformDirection(Vector3.forward).normalized, ForceMode.Impulse);
            grabbedObject = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_grabSearchOrigin.position, grabRadius);
        
    }
}
