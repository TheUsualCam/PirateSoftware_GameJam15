using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleInputListener : MonoBehaviour
{
    public float movementSpeed = 1f;
    private Vector2 movement;

    private void OnEnable()
    {
        // Subscribe to the STATIC events with +=
        // The signature must match, if an event gives a float, the function must take a float
        
        // This one takes a Vector2, where X is horizontal and Y is vertical inputs. e.g pressing W sets Y to 1.
        // This function is only called when the key CHANGES, so that means we have to save the data and use Update for per-frame effects.
        GameInput.OnMoveChanged += OnMove;
        
        // These ones are buttons! Instead they have a "Start" and "End".
        // Think of it like "Start" sets a bool to true
        // and "End" sets a bool to false.
        GameInput.onLeftActionStart += OnLeftActionStart;
        GameInput.onLeftActionEnd += OnLeftActionEnd;
        
        GameInput.onRightActionStart += OnRightActionStart;
        GameInput.onRightActionEnd += OnRightActionEnd;
    }

    private void OnDisable()
    {
        // Unsubscribe to the STATIC events with -=
        // Always unsubscribe, if left these become null pointers! Bad! Causes crashes/Errors.
        GameInput.OnMoveChanged -= OnMove;
        
        GameInput.onLeftActionStart -= OnLeftActionStart;
        GameInput.onLeftActionEnd -= OnLeftActionEnd;
        
        GameInput.onRightActionStart -= OnRightActionStart;
        GameInput.onRightActionEnd -= OnRightActionEnd;
    }

    private void Update()
    {
        if (movement != Vector2.zero)
        {
            transform.position += Time.deltaTime * movementSpeed * (Vector3)movement;
        }
    }

    private void OnMove(Vector2 newMovement)
    {
        movement = newMovement;
    }

    private void OnLeftActionStart()
    {
        IncreaseSize();
    }
    
    private void OnLeftActionEnd()
    {
        ResetSize();
    }

    private void IncreaseSize()
    {
        transform.localScale = Vector3.one * 1.5f;
    }

    private void ResetSize()
    {
        transform.localScale = Vector3.one;
    }
    
    private void OnRightActionStart()
    {
        SetColor(Color.red);
    }
    
    private void OnRightActionEnd()
    {
        SetColor(Color.white);
    }

    private void SetColor(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
    }

}
