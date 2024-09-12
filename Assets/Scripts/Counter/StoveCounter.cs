using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<StateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressBarChangedArgs> OnProgressBarChanged;

    public class StateChangedEventArgs : EventArgs {
        public State state;
    }
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOsArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    public enum State {
        Idle,
        Frying,
        Fried,
        Burned
    }
    private State state;

    private float fryingTimer;

    private FryingRecipeSO fryingRecipeSO;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }
    private void Update() 
    {
        if(HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressBarChanged.Invoke(this, new IHasProgress.OnProgressBarChangedArgs()
                    {
                        progressBarNormalize = fryingTimer / fryingRecipeSO.fryTimerMax
                    });
                    if (fryingTimer > fryingRecipeSO.fryTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeForRecipeInput(GetKitchenObject().GetkitchenObjectSO());
                        OnStateChanged?.Invoke(this, new StateChangedEventArgs() { state = state });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;
                    OnProgressBarChanged.Invoke(this, new IHasProgress.OnProgressBarChangedArgs()
                    {
                        progressBarNormalize = burningTimer / burningRecipeSO.burningTimerMax
                    });
                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state = State.Burned;
                        burningTimer = 0f;
                        OnStateChanged?.Invoke(this, new StateChangedEventArgs() { state = state });
                        OnProgressBarChanged.Invoke(this, new IHasProgress.OnProgressBarChangedArgs()
                        {
                            progressBarNormalize =  0f
                        });
                    }
                    
                    break;
                case State.Burned:
                    break;
            }
        }
    }
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetkitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingTimer = 0f;
                    fryingRecipeSO = GetFryingReciptForRecipeInput(GetKitchenObject().GetkitchenObjectSO());
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
                state = State.Idle;
                fryingTimer = 0f;
                GetKitchenObject().SetKitchenObjectParent(player);
                OnProgressBarChanged.Invoke(this, new IHasProgress.OnProgressBarChangedArgs()
                {
                    progressBarNormalize = 0f
                });
                OnStateChanged?.Invoke(this, new StateChangedEventArgs() { state = state });

            }
        }
    }

    public override void InteractAlt(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetkitchenObjectSO()))
        {
            if(fryingRecipeSO != null) {
                state = State.Frying;
                OnStateChanged?.Invoke(this, new StateChangedEventArgs() { state = state });
                OnProgressBarChanged.Invoke(this, new IHasProgress.OnProgressBarChangedArgs() {
                    progressBarNormalize = fryingTimer / fryingRecipeSO.fryTimerMax
                }); 
            }
        }
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
    private BurningRecipeSO GetBurningRecipeForRecipeInput(KitchenObjectSO kitchenObjectSO)
    {
        foreach (var burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == kitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}