using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionButton;



    private void Start()
    {
        KitchenGameManager.Instance.OnPauseAction += KitchenGameManager_Pause;
        KitchenGameManager.Instance.OnUnPauseAction += KitchenGameManager_Unpause;
        
        Hide();  // Hide the pause UI when the game starts.
    }

    private void KitchenGameManager_Unpause(object sender, EventArgs e)
    {
        Hide();  // 
    } 

    private void KitchenGameManager_Pause(object sender, EventArgs e)
    {
        Show(); //
    }

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.LoadScene(Loader.Sence.MainMenuScene);  // Load the main scene when the play button is clicked.   
        });
        resumeButton.onClick.AddListener(() =>
        {
            KitchenGameManager.Instance.UnPauseGame();  // Unpause the game when the quit button is clicked.
            EventSystem.current.SetSelectedGameObject(null);
        });
        optionButton.onClick.AddListener(() =>
        {
            OptionUI.Instance.Show();
        });
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
