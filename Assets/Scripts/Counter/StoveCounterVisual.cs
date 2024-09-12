using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    private const string CUT = "Cut";
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveGameObject;
    [SerializeField] private GameObject particlesGameObject;

    private Animator animator;

    private void Start()
    {
        stoveCounter.OnStateChanged += OnStoveCounterStateChanged;
    }

    private void OnStoveCounterStateChanged(object sender, StoveCounter.StateChangedEventArgs e)
    {
        bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        stoveGameObject.SetActive(showVisual);
        particlesGameObject.SetActive(showVisual); 
    }
}
