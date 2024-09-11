using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOsArray;
    private float cookingProcess;
    
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetkitchenObjectSO()))
                {
                    cookingProcess = 0;
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    // cuttingProgress = 0;
                    // FryingRecipeSO fryingRecipeSO = GetFryingReciptForRecipeInput(GetKitchenObject().GetkitchenObjectSO());
                    // OnProgressBarChanged?.Invoke(this, new OnProgressBarChangedArgs() { progressBarNormalize = (float)cuttingProgress / FryingRecipeSO.cuttingProgressMax });
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

            }
            else
            {
                // FryingRecipeSO fryingRecipeSO = GetFryingReciptForRecipeInput(GetKitchenObject().GetkitchenObjectSO());
                cookingProcess = 0;
                // OnProgressBarChanged?.Invoke(this, new OnProgressBarChangedArgs() { progressBarNormalize = 0f });
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlt(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetkitchenObjectSO()))
        {
            FryingRecipeSO fryingRecipeSO = GetFryingReciptForRecipeInput(GetKitchenObject().GetkitchenObjectSO());
            if(fryingRecipeSO != null) {
                StartCoroutine(Cooking(fryingRecipeSO));
            }
        }
    }

    private IEnumerator Cooking(FryingRecipeSO fryingRecipeSO)
    {
        while (cookingProcess < fryingRecipeSO.fryTimerMax)
        {
            cookingProcess++;
            yield return new WaitForSeconds(1);
        }
        GetKitchenObject().DestroySelf();
        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingReciptForRecipeInput(kitchenObjectSO);
        return fryingRecipeSO != null;
    }


    private FryingRecipeSO GetFryingReciptForRecipeInput(KitchenObjectSO kitchenObjectSO)
    {
        foreach (var fryingRecipeSO in fryingRecipeSOsArray)
        {
            if (fryingRecipeSO.input == kitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

}