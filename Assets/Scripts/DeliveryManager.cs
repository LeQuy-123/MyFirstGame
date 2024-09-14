using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> watitngRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private void Awake()
    {
        Instance = this;
        watitngRecipeSOList = new List<RecipeSO>();
    }
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if (watitngRecipeSOList.Count < waitingRecipeMax) SpawnRecipe();
        }
    }
    private void SpawnRecipe()
    {
        int randomIndex = Random.Range(0, recipeListSO.recipeSOList.Count);
        RecipeSO recipeSO = recipeListSO.recipeSOList[randomIndex];
        watitngRecipeSOList.Add(recipeSO);
        Debug.Log(recipeSO.name);
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
                    watitngRecipeSOList.RemoveAt(i); // Remove the matched recipe
                    Debug.Log("Match recipes");
                    Destroy(plateKitchenObject.gameObject); // Destroy the plate
                    return; // Exit the function after a match is found
                }
            }
        }
        // No matching recipe found
        Debug.Log("No match recipes");
    }

}
