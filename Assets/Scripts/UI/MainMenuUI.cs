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
            Loader.LoadScene(Loader.Sence.GameScene);  // Load the main scene when the play button is clicked.  // Replace "MainScene" with the name of your main scene.  // Make sure the scene is in the build settings.  // Also, make sure to add the SceneManager script to your main canvas.  // Make sure to have a SceneManager prefab in your project.  // You can also use the built-in Unity editor to add a SceneManager to your main canvas.  // If you use a SceneManager prefab, you can simply drag and drop it onto your main canvas.  // If you use a script, you can find it in the Assets/Scenes folder and drag and drop it onto your main canvas.  // If you use a script, you will need to add the necessary code to handle the scene loading.  // Make sure to have the necessary references and variables set up in your script.  // You can use the
        });
        quitButton.onClick.AddListener(() => {
            Application.Quit();  // Exit the game when the quit button is clicked.
        });
    }
}
