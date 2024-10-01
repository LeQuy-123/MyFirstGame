using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrash;
    
    new public static void ResetData()
    {
        OnAnyObjectTrash = null;
    }

    public override void  Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if(player.HasKitchenObject()) {
                KitchenObject.DestroyKitchenObject(player.GetKitchenObject());
                InteractLogicServerRpc();
            }
        } 
    }
    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }
    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        OnAnyObjectTrash?.Invoke(this, EventArgs.Empty);
    }
    
}
