using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngradientAddedEventArgs> OnIngradientAdded;
    public class OnIngradientAddedEventArgs : EventArgs {
        public KitchenObjectSO kitchenObjectSO;
    }
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    private List<KitchenObjectSO> kitchenObjectSOList;
    protected override  void Awake()
    {
        base.Awake();
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO) 
    {
        if (kitchenObjectSOList.Contains(kitchenObjectSO) || !validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        } 
        else 
        {
            TryAddIngredientServerRpc(KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObjectSO));
            return true;
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void TryAddIngredientServerRpc(int kitchenObjectSOIndex)
    {
        TryAddIngredientClientRpc(kitchenObjectSOIndex);
    }
    [ClientRpc]
    private void TryAddIngredientClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOByIndex(kitchenObjectSOIndex);
        kitchenObjectSOList.Add(kitchenObjectSO);
        OnIngradientAdded?.Invoke(this, new OnIngradientAddedEventArgs { kitchenObjectSO = kitchenObjectSO });
    }
    public List<KitchenObjectSO> GetKitchenObjectSOList() 
    {
        return kitchenObjectSOList;
    }
}
