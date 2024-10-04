using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] Button hostButton;
    [SerializeField] Button clientButton;
    private void Awake()
    {
        hostButton.onClick.AddListener(() => {
            KitchenGameMultiplayer.Instance.StartHost();
            Hide();
        });
        clientButton.onClick.AddListener(() => {
            KitchenGameMultiplayer.Instance.StartClient();
            Hide();
        });
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
