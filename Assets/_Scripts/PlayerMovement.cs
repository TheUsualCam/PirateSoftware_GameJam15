
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Vector3 _movementDirection;
    private Rigidbody _rigidbody;
    
    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        GameInput.OnMoveChanged += OnMove;
    }

    private void OnDisable()
    {
        GameInput.OnMoveChanged -= OnMove;
    }

    // Update is called once per frame
    void Update()
    {
        if (_movementDirection != Vector3.zero)
        {
            Vector3 movementUpdate = Time.deltaTime * speed * _movementDirection;
            _rigidbody.MovePosition(transform.position + movementUpdate);
        }
    }

    void OnMove(Vector2 movement)
    {
        _movementDirection = new Vector3(movement.x, 0f, movement.y);
    }
}
