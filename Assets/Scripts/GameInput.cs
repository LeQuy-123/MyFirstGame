using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_REF_BINDING = "Player_ref_binding";
    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlt,
        Pause,
    }
    public static GameInput Instance { get; private set; }
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAltAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingsChanged;
    
    private PlayerInputActions playerInputActions;
    private Camera mainCamera;
    private Vector3 targetPosition;
    private bool isMovingToTarget;

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += InteractPerformed;
        playerInputActions.Player.InteractAlt.performed += InteractAltPerformed;
        playerInputActions.Player.Pause.performed += PausePerformed;

        if (PlayerPrefs.HasKey(PLAYER_REF_BINDING))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_REF_BINDING));
        }
        // Subscribe to the click event
        // playerInputActions.Player.Click.performed += OnClick;

        // Assign the main camera
        mainCamera = Camera.main;
    }
    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= InteractPerformed;
        playerInputActions.Player.InteractAlt.performed -= InteractAltPerformed;
        playerInputActions.Player.Pause.performed -= PausePerformed;
        playerInputActions.Dispose();
    }
    private void PausePerformed(InputAction.CallbackContext context)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAltPerformed(InputAction.CallbackContext context)
    {
        OnInteractAltAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        // Cast a ray from the camera to the clicked point in the world
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            targetPosition = hit.point;         // Store the target position
            isMovingToTarget = true;            // Set flag to move towards the target
        }
        else
        {
        }
    }

    // Call this method from the Player script to get the target position
    public Vector3 GetTargetPosition()
    {
        return targetPosition;
    }

    // Check if the player should move towards the target
    public bool IsMovingToTarget()
    {
        return isMovingToTarget;
    }

    // Stop the movement when the player reaches the target
    public void StopMovingToTarget()
    {
        isMovingToTarget = false;
    }

    public string GetBidingText(Binding binding)
    {
        switch (binding)
        {
            case Binding.Move_Up:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlt:
                return playerInputActions.Player.InteractAlt.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
        }
        return "";
    }
    public void RebindBinding(Binding binding, Action action)
    {
        playerInputActions.Player.Disable();
        InputAction inputAction;
        int bindingIndex;
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlt:
                inputAction = playerInputActions.Player.InteractAlt;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
            
        }
        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback =>
                {
                    callback.Dispose();
                    playerInputActions.Player.Enable();
                    action();
                    PlayerPrefs.SetString(PLAYER_REF_BINDING, playerInputActions.SaveBindingOverridesAsJson());
                    OnBindingsChanged?.Invoke(this, EventArgs.Empty);
                }).Start();

    }
}

