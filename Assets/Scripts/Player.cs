using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnPlayerSpawned;
    public static event EventHandler OnPlayerPickUpSomething;

    public static Player LocalInstace { get; private set; }
    public event EventHandler OnPickupSomething;

    public event EventHandler<OnSelectedCountersChangedEventArgs> OnSelectedCountersChanged;
    public class OnSelectedCountersChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    [SerializeField] private float moveSpeed = 7f;
    // [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countrerLayerMask;
    [SerializeField] private LayerMask collisionsLayerMask;
    [SerializeField] private List<Vector3> spawnLocationList;

    private bool isWalking;
    private readonly float playerRadius = .7f;
    // private readonly float playerHeight = 2f;
    private Vector3 lastInteractDirection;
    private BaseCounter selectedCounter;
    public override void OnNetworkSpawn()
    {
        // base.OnNetworkSpawn();
        if(IsOwner)
        {
            LocalInstace = this;
        }
        transform.position = spawnLocationList[(int)OwnerClientId];
        OnPlayerSpawned.Invoke(this, EventArgs.Empty);
    }

    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAltAction += GameInput_OnInteractAltAction; 
    }

    private void GameInput_OnInteractAltAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlt(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            HandleMovement();
            // HandleMovementServerAuth();
            HandleInteraction();
        }
    }

    //this is for server movement auth, client send request to server then server move the player
    // private void HandleMovementServerAuth()
    // {
    //     Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
    //     HandleMovementServerRPC(inputVector);
    // }
    // [ServerRpc(RequireOwnership =false)]
    // private void HandleMovementServerRPC(Vector2 inputVector)
    // {
    //     Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);
    //     // Normal input movement with keyboard
    //     Move(moveDir);
    //     isWalking = moveDir != Vector3.zero;
    // }

    private void HandleInteraction()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
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
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);


        // Normal input movement with keyboard
        Move(moveDir);
        isWalking = moveDir != Vector3.zero;
    }

    // Move the player in the given direction
    private void Move(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            float moveDistance = Time.deltaTime * moveSpeed;
            bool canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, direction, Quaternion.identity, moveDistance, collisionsLayerMask);
            if (canMove)
            {
                transform.position += moveDistance * direction;
            }
            else
            {
                // attempt move only to the x direction
                Vector3 newDirectionX = new Vector3(direction.x, 0, 0).normalized;
                bool canMoveX = direction.x != 0 && !Physics.BoxCast(transform.position, Vector3.one * playerRadius, newDirectionX, Quaternion.identity, moveDistance, collisionsLayerMask);
                if (canMoveX)
                {
                    transform.position += moveDistance * newDirectionX;
                }
                else
                {
                    Vector3 newDirectionZ = new Vector3(0, 0, direction.z).normalized;
                    bool canMoveZ = direction.z != 0 && !Physics.BoxCast(transform.position, Vector3.one * playerRadius, newDirectionZ, Quaternion.identity, moveDistance, collisionsLayerMask);
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
        if (kitchenObject != null) {
            OnPickupSomething?.Invoke(this, EventArgs.Empty);
            OnPlayerPickUpSomething?.Invoke(this, EventArgs.Empty);
        } 
    }
    public static void ResetData()
    {
        OnPlayerSpawned = null;
        OnPlayerPickUpSomething=null;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
