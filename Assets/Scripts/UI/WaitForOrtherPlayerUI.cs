using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForOrtherPlayerUI : MonoBehaviour
{
    private void Start()
    {
        KitchenGameManager.Instance.OnLocalPlayerReadyChange += KitchenGameManager_OnLocalPlayerReadyChange;
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        Hide();
    }

    private void KitchenGameManager_OnLocalPlayerReadyChange(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.GetIsLocalPlayerReady())
        {
            Show();
        }
    }
    private void KitchenGameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountDownToStartActive())
        {
            Hide(); // Hide
        }
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
