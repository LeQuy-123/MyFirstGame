using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngradientAddedEventArgs> OnIngradientAdded;
    public class OnIngradientAddedEventArgs : EventArgs {
        public KitchenObjectSO kitchenObjectSO;
    }
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
            OnIngradientAdded?.Invoke(this, new OnIngradientAddedEventArgs{kitchenObjectSO = kitchenObjectSO});
            kitchenObjectSOList.Add(kitchenObjectSO);
            return true;
        }
    }
    public List<KitchenObjectSO> GetKitchenObjectSOList() 
    {
        return kitchenObjectSOList;
    }
}
