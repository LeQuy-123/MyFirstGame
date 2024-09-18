using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public static OptionUI Instance { get; private set; }

    [SerializeField] private Button soundButton;
    [SerializeField] private TextMeshProUGUI soundButtonText;
    [SerializeField] private Button musicButton;
    [SerializeField] private TextMeshProUGUI musicButtonText;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        Instance = this;
        soundButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateSoundButtonVisual();
        });
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateMusicButtonVisual();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    private void KitchenGameManager_OnUnPauseAction(object sender, EventArgs e)
    {
        Hide();
    }

    private void UpdateSoundButtonVisual ()
    {
        soundButtonText.text ="Sound Effect: "+ Math.Round(SoundManager.Instance.GetVolume() * 10f).ToString();
    }
    private void UpdateMusicButtonVisual()
    {
        musicButtonText.text = "Music: " + Math.Round(MusicManager.Instance.GetVolume() * 10f).ToString();
    }
    private void Start()
    {
        KitchenGameManager.Instance.OnUnPauseAction += KitchenGameManager_OnUnPauseAction;
        UpdateSoundButtonVisual();  // Update the sound effect button text.
        UpdateMusicButtonVisual();  // Update the music button text.
        Hide();  // Hide the pause UI when the game starts.
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
