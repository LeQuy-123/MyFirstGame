using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private LobbyCreateUI lobbyCreateUI;
    [SerializeField] private TMP_InputField lobbyCodeField;
    [SerializeField] private Button joinByCodeButton;


    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.LoadScene(Loader.Sence.MainMenuScene);
        });
        quickJoinButton.onClick.AddListener(() =>
        {
            // Quick join the game
            KitchenGameLobby.Instance.QuickJoinLobby();
        });
        createLobbyButton.onClick.AddListener(() =>
        {
            // Create a new lobby
            lobbyCreateUI.Show();
        });
        joinByCodeButton.onClick.AddListener(() =>
        {
            // Join a lobby by code
            KitchenGameLobby.Instance.JoinWithCodeLobby(lobbyCodeField.text);
        });
    }
}
