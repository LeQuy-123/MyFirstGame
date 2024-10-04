using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionUI : MonoBehaviour
{
    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnTryToJoinGame += KitchenGameMultiplayer_OnTryToJoinGame;
        KitchenGameMultiplayer.Instance.OnFaildToJoinGame += KitchenGameMultiplayer_OnFaildToJoinGame;
        Hide();
    }
    private void KitchenGameMultiplayer_OnFaildToJoinGame(object sender, EventArgs e)
    {
        Hide();
    }
    private void KitchenGameMultiplayer_OnTryToJoinGame(object sender, EventArgs e)
    {
        Show();
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnTryToJoinGame -= KitchenGameMultiplayer_OnTryToJoinGame;
        KitchenGameMultiplayer.Instance.OnFaildToJoinGame -= KitchenGameMultiplayer_OnFaildToJoinGame;
    }
}
