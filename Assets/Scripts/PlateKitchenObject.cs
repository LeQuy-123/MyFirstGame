using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    private List<KitchenObjectSO> kitchenObjectSOList;
    public  void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO) 
    {
        if (kitchenObjectSOList.Contains(kitchenObjectSO) || !validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        } else {
            kitchenObjectSOList.Add(kitchenObjectSO);
            return true;
        }
    }
}
