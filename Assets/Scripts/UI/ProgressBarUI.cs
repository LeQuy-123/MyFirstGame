using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;
    private IHasProgress hasProgress;
    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if(hasProgress == null) 
        {
            Debug.LogError("Object does not implement IHasProgress" + hasProgressGameObject);
            return;
        }
        hasProgress.OnProgressBarChanged += HasProgress_OnProgressBarChanged;
        barImage.fillAmount = 0f;
        Hide();
    }

    private void HasProgress_OnProgressBarChanged(object sender, IHasProgress.OnProgressBarChangedArgs e)
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
