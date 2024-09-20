using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public static OptionUI Instance { get; private set; }
    [SerializeField] TextMeshProUGUI keyMoveUpText;
    [SerializeField] TextMeshProUGUI keyMoveDownText;
    [SerializeField] TextMeshProUGUI keyMoveLeftText;
    [SerializeField] TextMeshProUGUI keyMoveRightText;
    [SerializeField] TextMeshProUGUI keyInteractText;
    [SerializeField] TextMeshProUGUI keyInteractAltText;
    [SerializeField] TextMeshProUGUI keyPauseText;
    [SerializeField] TextMeshProUGUI keyMoveGamePadText;
    [SerializeField] TextMeshProUGUI keyInteractGamePadText;
    [SerializeField] TextMeshProUGUI keyInteractAltGamePadText;
    [SerializeField] TextMeshProUGUI keyPauseGamePadText;

    private void Start()
    {
        GameInput.Instance.OnBindingsChanged += GameInput_OnBindingsChanged;
        KitchenGameManager.Instance.OnStateChanged +=KitchenGameManager_OnStateChanged;
        UpdateVisual();
        Show();  // Hide the pause UI when the game starts.
    }

    private void KitchenGameManager_OnStateChanged(object sender, EventArgs e)
    {
        if(KitchenGameManager.Instance.IsCountDownToStartActive())
        {
            Hide(); // Hide
        }
    }

    private void GameInput_OnBindingsChanged(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        keyMoveUpText.text = GameInput.Instance.GetBidingText(GameInput.Binding.Move_Up);
        keyMoveDownText.text = GameInput.Instance.GetBidingText(GameInput.Binding.Move_Down);
        keyMoveLeftText.text = GameInput.Instance.GetBidingText(GameInput.Binding.Move_Left);
        keyMoveRightText.text = GameInput.Instance.GetBidingText(GameInput.Binding.Move_Right);
        keyInteractText.text = GameInput.Instance.GetBidingText(GameInput.Binding.Interact);
        keyInteractAltText.text = GameInput.Instance.GetBidingText(GameInput.Binding.InteractAlt);
        keyPauseText.text = GameInput.Instance.GetBidingText(GameInput.Binding.Pause);
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
