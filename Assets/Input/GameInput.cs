using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private InputActions _inputActions;
    
    [Header("Movement")]
    [Tooltip("Keyboard\nWASD, Arrow Keys\n\nController\nLeft Joystick")][SerializeField] private Vector2 move;
    public static event Action<Vector2> OnMoveChanged;
    [Tooltip("Keyboard\nSpacebar\n\nController\nSouth Button")][SerializeField] private bool jump;
    public static event Action onJumpStart;
    public static event Action onJumpEnd;

    [Header("Actions")] 
    [Tooltip("Keyboard\nLeft Mouse Button\n\nController\nLeft Trigger")][SerializeField] private bool leftAction;
    public static event Action onLeftActionStart;
    public static event Action onLeftActionEnd;
    
    [Tooltip("Keyboard\nRight Mouse Button\n\nController\nRight Trigger")][SerializeField] private bool rightAction;
    public static event Action onRightActionStart;
    public static event Action onRightActionEnd;

    private void Awake()
    {
        _inputActions = new InputActions();
    }

    private void OnEnable()
    {
        // Movement
        _inputActions.Player.Move.Enable();
        _inputActions.Player.Move.performed += PerformMove;
        _inputActions.Player.Move.canceled += CancelMove;
        
        _inputActions.Player.Jump.Enable();
        _inputActions.Player.Jump.performed += PerformJump;
        _inputActions.Player.Jump.canceled += CancelJump;
        
        // Left Action
        _inputActions.Player.LeftAction.Enable();
        _inputActions.Player.LeftAction.performed += PerformLeftAction;
        _inputActions.Player.LeftAction.canceled += CancelLeftAction;
        
        // Right Action
        _inputActions.Player.RightAction.Enable();
        _inputActions.Player.RightAction.performed += PerformRightAction;
        _inputActions.Player.RightAction.canceled += CancelRightAction;
        
    }

    private void OnDisable()
    {
        // Movement
        _inputActions.Player.Move.Disable();
        _inputActions.Player.Move.performed -= PerformMove;
        _inputActions.Player.Move.canceled -= CancelMove;
        
        _inputActions.Player.Jump.Disable();
        _inputActions.Player.Jump.performed -= PerformJump;
        _inputActions.Player.Jump.canceled -= CancelJump;
        
        // Left Action
        _inputActions.Player.LeftAction.Disable();
        _inputActions.Player.LeftAction.performed -= PerformLeftAction;
        _inputActions.Player.LeftAction.canceled -= CancelLeftAction;
        
        // Right Action
        _inputActions.Player.RightAction.Disable();
        _inputActions.Player.RightAction.performed -= PerformRightAction;
        _inputActions.Player.RightAction.canceled -= CancelRightAction;
    }

    private void PerformMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        OnMoveChanged?.Invoke(move);
    }

    private void CancelMove(InputAction.CallbackContext context)
    {
        move = Vector3.zero;
        OnMoveChanged?.Invoke(move);
    }
    
    private void PerformLeftAction(InputAction.CallbackContext context)
    {
        leftAction = true;
        onLeftActionStart?.Invoke();
    }
    
    private void CancelLeftAction(InputAction.CallbackContext context)
    {
        leftAction = false;
        onLeftActionEnd?.Invoke();
    }
    
    private void PerformRightAction(InputAction.CallbackContext context)
    {
        rightAction = true;
        onRightActionStart?.Invoke();
    }
    
    private void CancelRightAction(InputAction.CallbackContext context)
    {
        rightAction = false;
        onRightActionEnd?.Invoke();
    }
    
    private void PerformJump(InputAction.CallbackContext context)
    {
        jump = true;
        onJumpStart?.Invoke();
    }
    
    private void CancelJump(InputAction.CallbackContext context)
    {
        jump = false;
        onJumpEnd?.Invoke();
    }

    
}
