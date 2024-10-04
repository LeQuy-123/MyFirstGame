using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionRespondMessageUI : MonoBehaviour
{
    [SerializeField] Button closeButton;
    [SerializeField] TextMeshProUGUI messageText;
    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }
    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnFaildToJoinGame += KitchenGameMultiplayer_OnFaildToJoinGame;
        Hide();
    }

    private void KitchenGameMultiplayer_OnFaildToJoinGame(object sender, EventArgs e)
    {
        Show();
        messageText.text = NetworkManager.Singleton.DisconnectReason;
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
        KitchenGameMultiplayer.Instance.OnFaildToJoinGame -= KitchenGameMultiplayer_OnFaildToJoinGame;
    }
}
