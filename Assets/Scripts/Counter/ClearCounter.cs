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
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    } 
                } else {
                    if (GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject1)) 
                    {
                        if (plateKitchenObject1.TryAddIngredient(player.GetKitchenObject().GetkitchenObjectSO()))
                        {
                            KitchenObject.DestroyKitchenObject(player.GetKitchenObject());
                        }
                    }
                }
            } else {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    
}
