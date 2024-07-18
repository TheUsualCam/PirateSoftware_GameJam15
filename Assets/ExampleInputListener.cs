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
        GameInput.OnMoveChanged += OnMove;
        
        GameInput.onLeftActionStart += OnLeftActionStart;
        GameInput.onLeftActionEnd += OnLeftActionEnd;
        
        GameInput.onRightActionStart += OnRightActionStart;
        GameInput.onRightActionEnd += OnRightActionEnd;
    }

    private void OnDisable()
    {
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
