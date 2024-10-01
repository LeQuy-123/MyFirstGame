using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    public event EventHandler<IHasProgress.OnProgressBarChangedArgs> OnProgressBarChanged;
    public event EventHandler OnCut;
    public static event EventHandler OnAnyCut;
    new public static void ResetData()
    {
        OnAnyCut = null;
    }
    public class OnProgressBarChangedArgs: EventArgs {
        public float progressBarNormalize; 
    }
    private int cuttingProgress;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if(HasRecipeWithInput(player.GetKitchenObject().GetkitchenObjectSO())) {
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);
                    InteractLogicPlaceObjectOnCounterServerRpc();
                }
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetkitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else
            {
                // cuttingProgress = 0;
                // OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedArgs() { progressBarNormalize = 0f});
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceObjectOnCounterServerRpc()
    {
        InteractAltLogicPlaceObjectOnCounterClientRpc();
    }
    [ClientRpc]
    private void InteractAltLogicPlaceObjectOnCounterClientRpc()
    {
        cuttingProgress = 0;
        // CuttingRecipeSO cuttingRecipeSO = GetCuttingReciptForRecipeInput(kitchenObject.GetkitchenObjectSO());
        OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedArgs() { progressBarNormalize = 0f});
    }

    public override void InteractAlt(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetkitchenObjectSO()))
        {
            CutObjectServerRpc();
            CheckCuttingProgressDoneServerRpc();

        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void CutObjectServerRpc()
    {
        CutObjectClientRpc();
    }
    [ClientRpc]
    private void CutObjectClientRpc()
    {
        cuttingProgress++;
        CuttingRecipeSO cuttingRecipeSO = GetCuttingReciptForRecipeInput(GetKitchenObject().GetkitchenObjectSO());
        OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedArgs() { progressBarNormalize = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax });
        OnCut.Invoke(this, EventArgs.Empty);
        OnAnyCut?.Invoke(this, EventArgs.Empty);
    }
    [ServerRpc(RequireOwnership = false)]
    private void CheckCuttingProgressDoneServerRpc()
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingReciptForRecipeInput(GetKitchenObject().GetkitchenObjectSO());
        if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
        {
            KitchenObject.DestroyKitchenObject(GetKitchenObject());
            KitchenObject.SpawnKitchenObject(cuttingRecipeSO.output, this);
        }
    }
    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingReciptForRecipeInput(kitchenObjectSO);
        return cuttingRecipeSO != null;
    }
 
    private CuttingRecipeSO GetCuttingReciptForRecipeInput(KitchenObjectSO kitchenObjectSO)
    {
        foreach (var cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == kitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
