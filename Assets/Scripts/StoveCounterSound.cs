using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;
    private float warningSoundPlayTime;
    private float maxWarningSoundPlayTime = .2f;
    private bool  playWarningSound;

    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressBarChanged += StoveCounter_OnProgressBarChanged;

    }
    private void Update()
    {
        if (playWarningSound)
        {
            warningSoundPlayTime -= Time.deltaTime;
            if (warningSoundPlayTime <= 0)
            {
                warningSoundPlayTime = maxWarningSoundPlayTime;
                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
    }
    private void StoveCounter_OnProgressBarChanged(object sender, IHasProgress.OnProgressBarChangedArgs e)
    {
        float burnShowProgressAmount = .5f;
        playWarningSound = stoveCounter.IsFried() && e.progressBarNormalize > burnShowProgressAmount;

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
