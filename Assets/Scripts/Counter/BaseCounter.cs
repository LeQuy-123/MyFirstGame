using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyDrop;
    public static void ResetData()
    {
        OnAnyDrop = null;
    }

    [SerializeField] private Transform counterTopPoint;

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
        return counterTopPoint;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnAnyDrop?.Invoke(this,EventArgs.Empty);
        }
    }
    public virtual void Interact(Player player)
    {
        
    }
    public virtual void InteractAlt(Player player)
    {

    }
    public NetworkObject GetNetworkObject()
    {
        return null;
    }
}
