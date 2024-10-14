using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField lobbyNameField;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button createPrivateButton;
    [SerializeField] private Button createPublicButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
        createPrivateButton.onClick.AddListener(() =>
        {
            // Quick join the game
            KitchenGameLobby.Instance.CreateLobby(lobbyNameField.text, true);
        });
        createPublicButton.onClick.AddListener(() =>
        {
            // Create a new lobby
            KitchenGameLobby.Instance.CreateLobby(lobbyNameField.text, false);
        });
    }
    private void Start()
    {
        Hide();
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
