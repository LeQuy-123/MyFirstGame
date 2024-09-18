using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_REF_SOUND_VOLUME = "PLAYER_REF_SOUND_VOLUME";
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    private float gameVol = 1f;
    private void Awake()
    {
        Instance = this;
        gameVol = PlayerPrefs.GetFloat(PLAYER_REF_SOUND_VOLUME, 1f);
    }
    private void Start()
    {
        // Subscribe to events
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailure += DeliveryManager_OnRecipeFailure;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickupSomething += Player_OnPickupSomething;
        BaseCounter.OnAnyDrop += BaseCounter_OnAnyDrop;
        TrashCounter.OnAnyObjectTrash += TrashCounter_OnAnyObjectTrash;

    }

    private void TrashCounter_OnAnyObjectTrash(object sender, EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaysoundRandom(audioClipRefsSO.objectPickup, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyDrop(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaysoundRandom(audioClipRefsSO.objectPickup, baseCounter.transform.position);
    }

    private void Player_OnPickupSomething(object sender, EventArgs e)
    {
        PlaysoundRandom(audioClipRefsSO.objectDrop, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaysoundRandom(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailure(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaysoundRandom(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaysoundRandom(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }
    private void PlaysoundRandom(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        Playsound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volume);
    }
    private void Playsound(AudioClip audioClip, Vector3 position, float volumeMultiple = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiple * gameVol);
    }
    public void PlayWalkingsound(Vector3 position, float volume = 1f)
    {
        PlaysoundRandom(audioClipRefsSO.footStep, position, volume);
    }
    public void ChangeVolume()
    {
        gameVol += .1f;
        if (gameVol > 1f)
        {
            gameVol = 0f;
        }
        PlayerPrefs.SetFloat(PLAYER_REF_SOUND_VOLUME, gameVol);
        PlayerPrefs.Save();
    }
    public float GetVolume()
    {
        return gameVol;
    }
}
