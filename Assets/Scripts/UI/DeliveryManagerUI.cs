using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;
    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeSpawned(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }
        List<RecipeSO> deliveryList = DeliveryManager.Instance.GetWaitingRecipeSOList();
        foreach (RecipeSO recipeSO in deliveryList)
        {
            Transform recipeTranform = Instantiate(recipeTemplate, container);
            recipeTranform.gameObject.SetActive(true);
            recipeTranform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }
}
