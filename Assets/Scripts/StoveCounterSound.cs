using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.StateChangedEventArgs e)
    {
        bool playsound = e.state == StoveCounter.State.Fried || e.state == StoveCounter.State.Frying;
        if (playsound)
        {
            audioSource.Play();
        } else {
            audioSource.Pause();
        }
    }
}
