using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        keyMoveUpText.text = GameInput.Instance.GetBidingText(GameInput.Binding.Move_Up);
        keyMoveDownText.text = GameInput.Instance.GetBidingText(GameInput.Binding.Move_Down);
        keyMoveLeftText.text = GameInput.Instance.GetBidingText(GameInput.Binding.Move_Left);
        keyMoveRightText.text = GameInput.Instance.GetBidingText(GameInput.Binding.Move_Right);
        keyInteractText.text = GameInput.Instance.GetBidingText(GameInput.Binding.Interact);
        keyInteractAltText.text = GameInput.Instance.GetBidingText(GameInput.Binding.InteractAlt);
        keyPauseText.text = GameInput.Instance.GetBidingText(GameInput.Binding.Pause);
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
