using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void  Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if(player.HasKitchenObject()) {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            } else {

            }
        } else {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetkitchenObjectSO())) 
                    {
                        GetKitchenObject().DestroySelf();
                    } 
                } else {
                    if (GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject1)) 
                    {
                        if (plateKitchenObject1.TryAddIngredient(player.GetKitchenObject().GetkitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            } else {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    
}
