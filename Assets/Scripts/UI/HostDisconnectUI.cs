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
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        Hide();
    }
    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)  // this will only be call when this local client disconnects or the server disconnects, it will not be call when other client disconnects
    {
        if (!NetworkManager.Singleton.IsHost) // this will prevent this popup show uo on host when client disconnect
        {
            Show();
        }
    }

    public void Show()
    {
        gameObject?.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
