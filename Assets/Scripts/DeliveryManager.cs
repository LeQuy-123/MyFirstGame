using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class DeliveryManager : NetworkBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailure;

    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> watitngRecipeSOList;
    private float spawnRecipeTimer = 4f;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int successfulRecipeAmmount;

    private void Awake()
    {
        Instance = this;
        watitngRecipeSOList = new List<RecipeSO>();
    }
    private void Update()
    {
        if (IsServer) 
        {
            spawnRecipeTimer -= Time.deltaTime;
            if (KitchenGameManager.Instance.IsGamePlaying() && spawnRecipeTimer <= 0f)
            {
                spawnRecipeTimer = spawnRecipeTimerMax;
                if (watitngRecipeSOList.Count < waitingRecipeMax) SpawnRecipe();
            }
        }
    }
    [ClientRpc]
    private void SpawnNewWaitingRecipeClientRpc(int randomIndex)
    {
        RecipeSO recipeSO = recipeListSO.recipeSOList[randomIndex];
        watitngRecipeSOList.Add(recipeSO);
        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }
    private void SpawnRecipe()
    {
        int randomIndex = UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count);
        SpawnNewWaitingRecipeClientRpc(randomIndex);
         // Spawn the recipe visual here
    }
    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject)
    {
        List<KitchenObjectSO> deliveryBurger = plateKitchenObject.GetKitchenObjectSOList();
        for (int i = 0; i < watitngRecipeSOList.Count; i++)
        {
            RecipeSO recipeSO = watitngRecipeSOList[i];
            if (recipeSO.kitchenObjectList.Count == deliveryBurger.Count)
            {
                bool plateContentMatch = true;
                // Check if all ingredients from recipeSO match deliveryBurger
                foreach (KitchenObjectSO ingredient in recipeSO.kitchenObjectList)
                {
                    if (!deliveryBurger.Contains(ingredient))
                    {
                        plateContentMatch = false;
                        break; // No need to check further if one ingredient doesn't match
                    }
                }
                // If all ingredients match, deliver the recipe
                if (plateContentMatch)
                {
                    DeliveryCorrectRecipeServerRpc(i);
                    Destroy(plateKitchenObject.gameObject); // Destroy the plate
                    return; // Exit the function after a match is found
                }
            }
        }
        // No matching recipe found
        DeliveryIncorrectRecipeServerRpc();
    }
    [ServerRpc(RequireOwnership =false)]
    private void DeliveryIncorrectRecipeServerRpc()
    {
        DeliveryIncorrectRecipeClientRpc();
    }
    [ClientRpc]
    private void DeliveryIncorrectRecipeClientRpc()
    {
        OnRecipeFailure?.Invoke(this, EventArgs.Empty); // Notify to play sounds
    }
    [ServerRpc(RequireOwnership = false)]
    private void DeliveryCorrectRecipeServerRpc(int watitngRecipeIndex)
    {
        DeliveryCorrectRecipeClientRpc(watitngRecipeIndex);
    }
    [ClientRpc]
    private void DeliveryCorrectRecipeClientRpc(int watitngRecipeIndex)
    {
        successfulRecipeAmmount++;
        watitngRecipeSOList.RemoveAt(watitngRecipeIndex); // Remove the matched recipe
        OnRecipeCompleted?.Invoke(this, EventArgs.Empty); // Notify other listeners
        OnRecipeSuccess?.Invoke(this, EventArgs.Empty); // Notify to play sounds
    }
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return watitngRecipeSOList;
    }
    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipeAmmount;
    }
}
