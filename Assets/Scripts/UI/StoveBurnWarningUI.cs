using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;
    private void Start()
    {
        stoveCounter.OnProgressBarChanged += StoveCounter_OnProgressBarChanged;
        Hide();
    }

    private void StoveCounter_OnProgressBarChanged(object sender, IHasProgress.OnProgressBarChangedArgs e)
    {
        float burnShowProgressAmount = .5f;
        bool show = e.progressBarNormalize > burnShowProgressAmount && stoveCounter.IsFried();
        if (show)
        {
            Show();
        } else {
            Hide();
        }
    }

    private void Show()
    {
        this.gameObject.SetActive(true);
    }
    private void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
