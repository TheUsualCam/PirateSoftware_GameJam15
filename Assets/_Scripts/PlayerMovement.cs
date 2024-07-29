
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotationStep;
    public Animator animator;
    private Vector3 _movementDirection;
    private Rigidbody _rigidbody;
    private PlayerPowerUp _powerUp;
    
    private void OnEnable()
    {
        
        _rigidbody = GetComponent<Rigidbody>();
        _powerUp = GetComponent<PlayerPowerUp>();
        GameInput.OnMoveChanged += OnMove;
    }

    private void OnDisable()
    {
        GameInput.OnMoveChanged -= OnMove;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_movementDirection != Vector3.zero)
        {
            float calculatedSpeed = speed * _powerUp.GetPowerUpSpeedMultiplier();
            Vector3 movementUpdate = Time.deltaTime * calculatedSpeed * _movementDirection;
            //Quaternion movementRotation = Quaternion.Slerp(Quaternion.Euler(transform.forward), Quaternion.Euler(_movementDirection), rotationStep * Time.deltaTime);
            _rigidbody.AddForce(movementUpdate, ForceMode.VelocityChange);
            
            // Rotate to face velocity
            transform.forward = Vector3.Lerp(transform.forward, _movementDirection, rotationStep * Time.fixedDeltaTime);
            
        }
        animator.SetFloat("SpeedZ", _rigidbody.velocity.magnitude);
    }

    void OnMove(Vector2 movement)
    {
        _movementDirection = new Vector3(movement.x, 0f, movement.y);
    }
}
