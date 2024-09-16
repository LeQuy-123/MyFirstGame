using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrash;

    public override void  Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if(player.HasKitchenObject()) {
                player.GetKitchenObject().DestroySelf();
                OnAnyObjectTrash?.Invoke(this, EventArgs.Empty);
            } else {

            }
        } 
    }

    
}
