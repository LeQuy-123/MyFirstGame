using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] private Button soundButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        soundButton.onClick.AddListener(() =>
        {
            
        });
        musicButton.onClick.AddListener(() =>
        {
            
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }
    private void Start()
    {
        Hide();  // Hide the pause UI when the game starts.
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
