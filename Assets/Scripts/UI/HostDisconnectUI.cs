using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDisconnectUI : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    private void Awake()
    {
        restartButton.onClick.AddListener(() => {
            NetworkManager.Singleton.Shutdown();
            Loader.LoadScene(Loader.Sence.MainMenuScene);
        });
    }
    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        Hide();
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        if (clientId == NetworkManager.ServerClientId)
        {
            Show();
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
