using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }
    [SerializeField] PlateKitchenObject plateKitchenObject;
    [SerializeField] List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;

    private void Start()
    {
        plateKitchenObject.OnIngradientAdded += PlateKitchenObject_OnSelectedCountersChanged;
        foreach (var kitchenObjectSOGameObject in kitchenObjectSOGameObjectList)
        {
            kitchenObjectSOGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnSelectedCountersChanged(object sender, PlateKitchenObject.OnIngradientAddedEventArgs e)
    {
        foreach (var kitchenObjectSOGameObject in kitchenObjectSOGameObjectList)
        {
            if (kitchenObjectSOGameObject.kitchenObjectSO == e.kitchenObjectSO)
            {
                kitchenObjectSOGameObject.gameObject.SetActive(true);
                break;
            }
        }
    }
}
