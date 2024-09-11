using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjects;

    private void Start() 
    {
        Player.Instance.OnSelectedCountersChanged += Player_OnSelectedCountersChanged;
    }

    private void Player_OnSelectedCountersChanged(object sender, Player.OnSelectedCountersChangedEventArgs e)
    {
        if (e.selectedCounter == baseCounter) 
        {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() 
    {
        foreach (var visualGameObject in visualGameObjects)
        {
            visualGameObject.SetActive(true);
        }
    }
    private void Hide() 
    {
        foreach (var visualGameObject in visualGameObjects)
        {
            visualGameObject.SetActive(false);
        }
    }
}
