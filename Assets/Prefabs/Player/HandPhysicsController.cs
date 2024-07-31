using System.Collections;
using System.Collections.Generic;
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
    [Header("Throwing")] 
    [SerializeField] private float throwWindUpDuration;
    [SerializeField] private float windUpStrength;
    [SerializeField] private Vector3 windUpDirection;
    // The Speed the hands should be moving before the object is thrown
    [SerializeField] private float throwDuration;
    [SerializeField] private float throwStrength;
    [SerializeField] private float thrownObjectImpulseStrength;
    [SerializeField] private Vector3 throwDirection;
    [SerializeField] private bool isThrowing;

    public AudioClip pickUpClip;
    public AudioClip dropClip;
    public AudioClip throwClip;



    private void OnEnable()
    {
        GameInput.onLeftActionStart += StartGrab;
        GameInput.onLeftActionEnd += EndGrab;
        GameInput.onRightActionStart += StartThrow;
        Station.OnIngredientProcessingStarted += CheckIfGrabbedIngredientHasProcessed;

    }

    private void OnDisable()
    {
        GameInput.onRightActionStart -= StartThrow;
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
                AudioManager.instance.PlaySoundClip(pickUpClip, _balancer, 1f);
                return;
            }
        }
    }
    
    void EndGrab()
    {
        _isGrabActive = false;
        DropObject(dropForce * _balancer.TransformDirection(Vector3.forward).normalized);
    }
    
    private void CheckIfGrabbedIngredientHasProcessed(Ingredient obj)
    {
        if (obj.transform == grabbedObject)
        {
            EndGrab();
        }
    }

    public void StartThrow()
    {
        if (!_isGrabActive || !grabbedObject)
        {
            return;
        }
        
        _isGrabActive = false;
        isThrowing = true;
        AudioManager.instance.PlaySoundClip(throwClip, _balancer, 1f);

        StartCoroutine(EThrow());
    }

    private IEnumerator EThrow()
    {
        // Wind Up

        Vector3 throwDirectionRelative;
        float t = 0;
        
        while (isThrowing && t < throwWindUpDuration)
        {
            throwDirectionRelative = transform.TransformDirection(windUpDirection);
            
            leftHandRigidbody.AddForce(Time.deltaTime * windUpStrength * throwDirectionRelative, ForceMode.Force);
            rightHandRigidBody.AddForce(Time.deltaTime * windUpStrength * throwDirectionRelative, ForceMode.Force);
            
            t += Time.deltaTime;
            yield return null;
        }

        // Release and Launch
        t = 0f;
        while (isThrowing && t < throwDuration)
        {
            throwDirectionRelative = _balancer.transform.TransformDirection(throwDirection);
            
            leftHandRigidbody.AddForce(Time.deltaTime * throwStrength * throwDirectionRelative, ForceMode.Force);
            rightHandRigidBody.AddForce(Time.deltaTime * throwStrength * throwDirectionRelative, ForceMode.Force);

            t += Time.deltaTime;
            
            yield return null;
        }

        FinishThrow();
        
        yield return null;
    }

    public void FinishThrow()
    {
        isThrowing = false;
        DropObject(_balancer.transform.TransformDirection(throwDirection) * thrownObjectImpulseStrength);
    }

    void DropObject(Vector3 force)
    {
        if (grabbedObject)
        {
            grabbedObject.SetParent(null, true);
            Rigidbody grabbedRB = grabbedObject.gameObject.GetComponent<Rigidbody>();
            grabbedRB.isKinematic = false;
            grabbedRB.AddForce(force, ForceMode.Impulse);
            grabbedObject = null;
            AudioManager.instance.PlaySoundClip(dropClip, _balancer, 1f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_grabSearchOrigin.position, grabRadius);
        
    }
}
