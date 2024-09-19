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
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAltText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;

    private void Start()
    {
        KitchenGameManager.Instance.OnUnPauseAction += KitchenGameManager_OnUnPauseAction;
        UpdateSoundButtonVisual();  // Update the sound effect button text.
        UpdateMusicButtonVisual();  // Update the music button text.
        UpdateButtonVisual();
        Hide();  // Hide the pause UI when the game starts.
    }

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
        moveUpButton.onClick.AddListener(() => {
            moveUpText.text = "...";
            RebindBinding(GameInput.Binding.Move_Up);
        });
        moveDownButton.onClick.AddListener(() =>
        {
            moveDownText.text = "...";
            RebindBinding(GameInput.Binding.Move_Down);
        });
        moveLeftButton.onClick.AddListener(() =>
        {
            moveLeftText.text = "...";
            RebindBinding(GameInput.Binding.Move_Left);
        });
        moveRightButton.onClick.AddListener(() =>
        {
            moveRightText.text = "...";
            RebindBinding(GameInput.Binding.Move_Right);
        });
        interactButton.onClick.AddListener(() =>
        {
            interactText.text = "...";
            RebindBinding(GameInput.Binding.Interact);
        });
        interactAltButton.onClick.AddListener(() =>
        {
            interactAltText.text = "...";
            RebindBinding(GameInput.Binding.InteractAlt);
        });
        pauseButton.onClick.AddListener(() =>
        {
            pauseText.text = "...";
            RebindBinding(GameInput.Binding.Pause);
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
    private void UpdateButtonVisual()
    {
        moveUpText.text = GameInput.Instance.GetBidingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBidingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBidingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBidingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBidingText(GameInput.Binding.Interact);
        interactAltText.text = GameInput.Instance.GetBidingText(GameInput.Binding.InteractAlt);
        pauseText.text = GameInput.Instance.GetBidingText(GameInput.Binding.Pause);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    private void RebindBinding(GameInput.Binding binding)
    {
        GameInput.Instance.RebindBinding(binding, UpdateButtonVisual);
    }
}
