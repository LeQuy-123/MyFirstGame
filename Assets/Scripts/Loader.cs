using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader 
{
    public enum Sence {
        MainMenuScene,
        GameScene,
        LoadingScene,
        LobbyScene,
        CharacterSelectScene,
    }
    private static Sence targetSence;
    public static void LoadScene(Sence sceneName)
    {
        Loader.targetSence = sceneName;
        SceneManager.LoadScene(Sence.LoadingScene.ToString());
    }
    public static void LoadNetworkScene(Sence sceneName)
    {
        Loader.targetSence = sceneName;
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName.ToString(), LoadSceneMode.Single);
    }
    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetSence.ToString());
    }
}
