using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader 
{
    public enum Sence {
        MainMenuScene,
        GameScene,
        LoadingScene,
    }
    private static Sence targetSence;
    public static void LoadScene(Sence sceneName)
    {
        Loader.targetSence = sceneName;
        SceneManager.LoadScene(Sence.LoadingScene.ToString());
    }
    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetSence.ToString());
    }
}
