using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private Image barImage;

    private void Start()
    {
        cuttingCounter.OnProgressBarChanged += CuttingCounter_OnProgressBarChanged;
        barImage.fillAmount = 0f;
        Hide();
    }

    private void CuttingCounter_OnProgressBarChanged(object sender, CuttingCounter.OnProgressBarChangedArgs e)
    {
        barImage.fillAmount = e.progressBarNormalize;
        if(e.progressBarNormalize == 0f || e.progressBarNormalize == 1f) {
            Hide();
        } else {
            Show();
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
