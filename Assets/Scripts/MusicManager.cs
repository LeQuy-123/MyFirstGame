using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const string PLAYER_REF_MUSIC_VOLUME = "PLAYER_REF_MUSIC_VOLUME";
    public static MusicManager Instance { get; private set; }
    private AudioSource audioSource;
    private float gameVol = .3f;
    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        gameVol = PlayerPrefs.GetFloat(PLAYER_REF_MUSIC_VOLUME, .3f);
        audioSource.volume = gameVol;
    }
    public void ChangeVolume()
    {
        gameVol += .1f;
        if (gameVol > 1f)
        {
            gameVol = 0f;
        }
        audioSource.volume = gameVol;
        PlayerPrefs.SetFloat(PLAYER_REF_MUSIC_VOLUME, gameVol);
        PlayerPrefs.Save();
    }
    public float GetVolume()
    {
        return gameVol;
    }
}
