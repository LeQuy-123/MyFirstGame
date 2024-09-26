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
        Hide();
        if(Player.LocalInstace != null) {
            Player.LocalInstace.OnSelectedCountersChanged += Player_OnSelectedCountersChanged;
        } else {
            Player.OnPlayerSpawned += Player_OnPlayerSpawned;
        }
    }

    private void Player_OnPlayerSpawned(object sender, EventArgs e)
    {
        if (Player.LocalInstace != null)
        {
            Player.LocalInstace.OnSelectedCountersChanged -= Player_OnSelectedCountersChanged;
            Player.LocalInstace.OnSelectedCountersChanged += Player_OnSelectedCountersChanged;
        }
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
