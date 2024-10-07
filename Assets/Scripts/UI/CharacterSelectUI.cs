using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    [SerializeField] private Button readyButton;
    private void Awake()
    {
        menuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.LoadScene(Loader.Sence.MainMenuScene);
        });
        readyButton.onClick.AddListener(() =>
        {
            CharacterSelectReady.Instace.SetPlayerReady();
        });
    }

}
