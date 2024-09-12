using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateIconUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        plateKitchenObject.OnIngradientAdded += PlateKitchenObject_OnSelectedCountersChanged;
        
    }

    private void PlateKitchenObject_OnSelectedCountersChanged(object sender, PlateKitchenObject.OnIngradientAddedEventArgs e)
    {
        UpdateVisual();
        
    }

    private void UpdateVisual()
    {
        Debug.Log("UpdateVisual");
        foreach (Transform child in transform)
        {
            if (child != iconTemplate) 
            {
                Destroy(child.gameObject);
            }
        }
        List<KitchenObjectSO> kitchenObjectSOList = plateKitchenObject.GetKitchenObjectSOList();
        foreach (var kitchenObjectSO in kitchenObjectSOList)
        {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(false);
            iconTransform.GetComponent<PlateIconSingle>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
