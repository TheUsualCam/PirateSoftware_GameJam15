using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKController : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private bool _isIKActive;
    [SerializeField] private Transform rightHandTarget = null;
    [SerializeField] private Transform rightFootTarget = null;
    [SerializeField] private Transform leftFootTarget = null;
    [SerializeField] private Transform lookTarget = null;
    
    [SerializeField] private Rigidbody upwardsForceOrigin;
    [SerializeField] private Rigidbody downwardsForceOrigin;
    [SerializeField] private float upwardsForce = 0;
    [SerializeField] private float downwardsForce = 0;
    

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (upwardsForceOrigin)
        {
            upwardsForceOrigin.AddForce(Vector3.up * upwardsForce, ForceMode.Force);
        }
        if (downwardsForceOrigin)
        {
            downwardsForceOrigin.AddForce(Vector3.down * downwardsForce, ForceMode.Force);
        }
        
        
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (!_animator) return;
        
        if (_isIKActive)
        {
            // Look at IK
            _animator.SetLookAtWeight(lookTarget ? 1 : 0);
            _animator.SetLookAtPosition(lookTarget ? lookTarget.position : Vector3.zero);

            // Right Hand IK
            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandTarget ? 1 : 0);
            _animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget ? rightHandTarget.position : Vector3.zero);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandTarget ? 1 : 0);
            _animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget ? rightHandTarget.rotation : Quaternion.identity);
            
            // Right Foot IK
            _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootTarget ? 1 : 0);
            _animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootTarget ? rightFootTarget.position : Vector3.zero);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootTarget ? 1 : 0);
            _animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootTarget ? rightFootTarget.rotation : Quaternion.identity);
            
            // Left Foot IK
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootTarget ? 1 : 0);
            _animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootTarget ? leftFootTarget.position : Vector3.zero);
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootTarget ? 1 : 0);
            _animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootTarget ? leftFootTarget.rotation : Quaternion.identity);
        }
        else // Inactive State
        {
            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            
            _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
            
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);
        }
    }
}
