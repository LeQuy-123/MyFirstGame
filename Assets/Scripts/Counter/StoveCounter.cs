using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
    private NetworkVariable<State> state = new NetworkVariable<State>(State.Idle);
    private NetworkVariable<float> fryingTimer = new NetworkVariable<float>(0f);
    private FryingRecipeSO fryingRecipeSO;
    private NetworkVariable<float> burningTimer = new NetworkVariable<float>(0f);
    private BurningRecipeSO burningRecipeSO;

 
    public override void OnNetworkSpawn()
    {
        fryingTimer.OnValueChanged+= FryingTimer_OnValueChanged;
        burningTimer.OnValueChanged += BurningTimer_OnValueChanged;
        state.OnValueChanged += State_OnValueChanged;
    }
    private void BurningTimer_OnValueChanged(float previousValue, float newValue)
    {
        float burningTimerMax = burningRecipeSO != null? burningRecipeSO.burningTimerMax : 1f;
        OnProgressBarChanged.Invoke(this, new IHasProgress.OnProgressBarChangedArgs()
        {
            progressBarNormalize = burningTimer.Value / burningTimerMax
        });
    }

    private void FryingTimer_OnValueChanged(float previusValue, float newValue)
    {
        if(state.Value != State.Frying) return;
        float fryTimerMax = fryingRecipeSO != null ? fryingRecipeSO.fryTimerMax : 1f;
        OnProgressBarChanged.Invoke(this, new IHasProgress.OnProgressBarChangedArgs()
        {
            progressBarNormalize = fryingTimer.Value / fryTimerMax
        });
    }
    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this, new StateChangedEventArgs() { state = state.Value });
        if (state.Value == State.Burned || state.Value == State.Idle)
        {
            OnProgressBarChanged.Invoke(this, new IHasProgress.OnProgressBarChangedArgs()
            {
                progressBarNormalize = 0f
            });
        }
    }
    private void Update() 
    {
        if(!IsServer) return;
        if(HasKitchenObject())
        {
            switch (state.Value)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer.Value += Time.deltaTime;
                    if (fryingTimer.Value > fryingRecipeSO.fryTimerMax)
                    {
                        KitchenGameMultiplayer.Instance.DestroyKitchenObject(GetKitchenObject());
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        state.Value = State.Fried;
                        burningTimer.Value = 0f;
                        SetBurninfRecipeSOClientRpc(
                            KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(GetKitchenObject().GetkitchenObjectSO())
                        );
                    }
                    break;
                case State.Fried:
                    burningTimer.Value += Time.deltaTime;
                    if (burningTimer.Value > burningRecipeSO.burningTimerMax)
                    {
                        KitchenGameMultiplayer.Instance.DestroyKitchenObject(GetKitchenObject());
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state.Value = State.Burned;                        
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
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);
                    InteractLogicPlaceObjectOnCounterServerRpc(KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObject.GetkitchenObjectSO()));
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
                        KitchenGameMultiplayer.Instance.DestroyKitchenObject(GetKitchenObject());
                        SetStateIdleServerRpc();
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                SetStateIdleServerRpc();
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetStateIdleServerRpc()
    {
        state.Value = State.Idle;
    }
    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceObjectOnCounterServerRpc(int kitchenObjectIndex)
    {
        fryingTimer.Value = 0f;
        state.Value = State.Frying;
        SetFryingRecipeSOClientRpc(kitchenObjectIndex);
    }
    [ClientRpc]
    private void SetFryingRecipeSOClientRpc(int kitchenObjectIndex)
    {
        KitchenObjectSO kitchenObjectSo = KitchenGameMultiplayer.Instance.GetKitchenObjectSOByIndex(kitchenObjectIndex);
        fryingRecipeSO = GetFryingReciptForRecipeInput(kitchenObjectSo);
    }

    [ClientRpc]
    private void SetBurninfRecipeSOClientRpc(int kitchenObjectIndex)
    {
        KitchenObjectSO kitchenObjectSo = KitchenGameMultiplayer.Instance.GetKitchenObjectSOByIndex(kitchenObjectIndex);
        burningRecipeSO = GetBurningRecipeForRecipeInput(kitchenObjectSo);
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
    public bool IsFried()
    {
        return state.Value == State.Fried;
    }
}