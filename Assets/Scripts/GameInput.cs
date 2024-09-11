using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAltAction;

    private PlayerInputActions playerInputActions;
    private Camera mainCamera;
    private Vector3 targetPosition;
    private bool isMovingToTarget;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += InteractPerformed;
        playerInputActions.Player.InteractAlt.performed += InteractAltPerformed;

        // Subscribe to the click event
        // playerInputActions.Player.Click.performed += OnClick;

        // Assign the main camera
        mainCamera = Camera.main;
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
}
