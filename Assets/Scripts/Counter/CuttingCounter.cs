using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    public event EventHandler<IHasProgress.OnProgressBarChangedArgs> OnProgressBarChanged;
    public event EventHandler OnCut;

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
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingReciptForRecipeInput(GetKitchenObject().GetkitchenObjectSO());
                    OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedArgs() { progressBarNormalize = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax });
                }
            }
            else
            {

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
                cuttingProgress = 0;
                OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedArgs() { progressBarNormalize = 0f});
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlt(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetkitchenObjectSO()))
        {
            cuttingProgress++;
            CuttingRecipeSO cuttingRecipeSO = GetCuttingReciptForRecipeInput(GetKitchenObject().GetkitchenObjectSO());
            OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedArgs() { progressBarNormalize = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax });
            OnCut.Invoke(this, EventArgs.Empty);
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(cuttingRecipeSO.output, this);
            }
            
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
