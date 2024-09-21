using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnProgressBarFlashUI : MonoBehaviour
{
    private string IS_FLASHING = "IsFlashing";
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    [SerializeField] StoveCounter stoveCounter;
    private void Start()
    {
        stoveCounter.OnProgressBarChanged += StoveCounter_OnProgressBarChanged;
        animator.SetBool(IS_FLASHING, false);

    }

    private void StoveCounter_OnProgressBarChanged(object sender, IHasProgress.OnProgressBarChangedArgs e)
    {
        float burnShowProgressAmount = .5f;
        bool show = e.progressBarNormalize > burnShowProgressAmount && stoveCounter.IsFried();
        animator.SetBool(IS_FLASHING, show);
    }

 
}
