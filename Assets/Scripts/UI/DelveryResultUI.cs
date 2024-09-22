using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DelveryResultUI : MonoBehaviour
{
    private string POP_UP_TRIGGER = "DeliveryResultPopup";
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI textMessage;
    [SerializeField] private Image iconImage;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failureColor;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failureSprite;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        Hide();
        DeliveryManager.Instance.OnRecipeSuccess+= DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailure += DeliveryManager_OnRecipeFailure;
    }

    private void DeliveryManager_OnRecipeFailure(object sender, EventArgs e)
    {
        animator.SetTrigger(POP_UP_TRIGGER);
        textMessage.text = "DELIVERY\nFAILURE!";
        backgroundImage.color = failureColor;
        iconImage.sprite = failureSprite;
        Show();
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        animator.SetTrigger(POP_UP_TRIGGER);
        textMessage.text = "DELIVERY\nSUCCESS!";
        backgroundImage.color = successColor;
        iconImage.sprite = successSprite;
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
