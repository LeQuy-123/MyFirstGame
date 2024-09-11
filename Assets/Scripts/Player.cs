using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }
    public event EventHandler<OnSelectedCountersChangedEventArgs> OnSelectedCountersChanged;
    public class OnSelectedCountersChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;

    [SerializeField] private LayerMask countrerLayerMask;

    private bool isWalking;
    private readonly float playerRadius = .7f;
    private readonly float playerHeight = 2f;
    private Vector3 lastInteractDirection;
    private BaseCounter selectedCounter;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Something is happening");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAltAction += GameInput_OnInteractAltAction; 
    }

    private void GameInput_OnInteractAltAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlt(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);
        if (moveDir != Vector3.zero)
        {
            lastInteractDirection = moveDir;  // Store the direction for future reference in OnTriggerEnter
        }
        float interactDistance = 2f;
        bool isHitSomething = Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, countrerLayerMask);
        if (isHitSomething)
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter)
                {
                    SetBaseCounter(baseCounter);
                }
            }
            else
            {
                SetBaseCounter(null);
            }
        }
        else
        {
            SetBaseCounter(null);
        }
    }

    private void HandleMovement()
    {
        // Get input vector for manual movement (WASD)
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);

        // If no input from keyboard, check for mouse click movement
        if (moveDir == Vector3.zero && gameInput.IsMovingToTarget())
        {
            MoveToTarget();  // Move towards the clicked target position
        }
        else
        {
            // Normal input movement with keyboard
            Move(moveDir);
            isWalking = moveDir != Vector3.zero;

        }
    }

    // Move the player in the given direction
    private void Move(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            float moveDistance = Time.deltaTime * moveSpeed;
            bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, direction, moveDistance);
            if (canMove)
            {
                transform.position += moveDistance * direction;
            }
            else
            {
                // attempt move only to the x direction
                Vector3 newDirectionX = new Vector3(direction.x, 0, 0).normalized;
                bool canMoveX = direction.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, newDirectionX, moveDistance);
                if (canMoveX)
                {
                    transform.position += moveDistance * newDirectionX;
                }
                else
                {
                    Vector3 newDirectionZ = new Vector3(0, 0, direction.z).normalized;
                    bool canMoveZ = direction.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, newDirectionZ, moveDistance);
                    if (canMoveZ)
                    {
                        transform.position += moveDistance * newDirectionZ;
                    }
                }
            }
            transform.forward = Vector3.Slerp(transform.forward, direction, Time.deltaTime * 10f);
        }
    }

    // Move the player towards the clicked target position
    private void MoveToTarget()
    {
        Vector3 targetPosition = gameInput.GetTargetPosition();  // Get target position from GameInput
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        moveDir.y = 0f;  // Ensure movement is only on the horizontal plane

        // Move and rotate the player towards the target
        transform.position += moveSpeed * Time.deltaTime * moveDir;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * 10f);

        // Stop moving when the player is close to the target
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            gameInput.StopMovingToTarget();  // Stop moving when reaching the destination
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void SetBaseCounter(BaseCounter newCounter)
    {
        this.selectedCounter = newCounter;
        OnSelectedCountersChanged?.Invoke(this, new OnSelectedCountersChangedEventArgs
        {
            selectedCounter = newCounter
        });
    }


    //for interact with kitchen opbjects
    [SerializeField] private Transform playerTopPoint;

    private KitchenObject kitchenObject;

    public void ClearKitchenObject()
    {
        this.kitchenObject = null;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return playerTopPoint;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }
}
