using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class HandPhysicsController : MonoBehaviour
{
    private Animator _animator;
    public bool isLeftHand;
    public bool _isGrabActive = false;
    [SerializeField] private Transform grabbedObject;
    [SerializeField] private Transform _hipsTransform;
    [SerializeField] private Rigidbody _handRigidbody;
    [SerializeField] private float grabForce = 4000f;
    [SerializeField] private float grabRadius = 0.8f;

    private void OnEnable()
    {
        if (isLeftHand)
        {
            GameInput.onLeftActionStart += StartGrab;
            GameInput.onLeftActionEnd += EndGrab;
        }
        else
        {
            GameInput.onRightActionStart += StartGrab;
            GameInput.onRightActionEnd += EndGrab;
        }
    }

    private void OnDisable()
    {
        GameInput.onRightActionStart -= StartGrab;
        GameInput.onLeftActionStart -= StartGrab;
        GameInput.onRightActionEnd -= EndGrab;
        GameInput.onLeftActionEnd -= EndGrab;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (_isGrabActive)
        {
            Vector3 targetDirection = Vector3.zero;
            if (grabbedObject)
            {
                targetDirection = grabbedObject.position - _handRigidbody.transform.position;
            }
            else
            {
                targetDirection = _hipsTransform.forward;
            }
            _handRigidbody.AddForce(grabForce * Time.fixedDeltaTime * targetDirection.normalized, ForceMode.Force);
        }
    }

    void StartGrab()
    {
        _isGrabActive = true;
        Physics.SphereCast(new Ray(transform.position, Vector3.forward), grabRadius, out var hitInfo);
        if (hitInfo.transform != null && hitInfo.transform.CompareTag("Grabable"))
        {
            grabbedObject = hitInfo.transform;
        }
    }
    
    void EndGrab()
    {
        _isGrabActive = false;
        grabbedObject = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        Gizmos.DrawWireSphere(_handRigidbody.position, grabRadius);
    }
}
