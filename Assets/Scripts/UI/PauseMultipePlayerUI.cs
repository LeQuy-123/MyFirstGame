using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMultipePlayerUI : MonoBehaviour
{

    private void Start()
    {
        KitchenGameManager.Instance.OnMultiplayerPauseAction += KitchenGameManager_OnMultiplayerPauseAction;
        KitchenGameManager.Instance.OnMultiplayerUnPauseAction += KitchenGameManager_OnMultiplayerUnPauseAction;

        Hide();  // Hide the pause UI when the game starts.
    }

    private void KitchenGameManager_OnMultiplayerUnPauseAction(object sender, EventArgs e)
    {
        Hide();
    }

    private void KitchenGameManager_OnMultiplayerPauseAction(object sender, EventArgs e)
    {
        Show();
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
