using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{ 
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    private void Awake()
    {
        playButton.onClick.AddListener(() => {
            Loader.LoadScene(Loader.Sence.LobbyScene);  // Load the main scene when the play button is clicked.   
        });
        quitButton.onClick.AddListener(() => {
            Application.Quit();  // Exit the game when the quit button is clicked.
        });
        Time.timeScale = 1f;
    }
}
