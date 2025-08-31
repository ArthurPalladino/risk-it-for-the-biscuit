using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalText;
    [SerializeField] Image finalImage;

    [SerializeField] Button closeButton;


    void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            finalImage.gameObject.SetActive(false);
            finalText.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(false);
            
        });
    }

    public void SetAction(Action action)
    {
        closeButton.onClick.AddListener(() => action?.Invoke());
    }
    void SetFinalText(string text)
    {
        finalText.text = text;
    }

    public void ActivateFinalScreen(string text)
    {
        SetFinalText(text);
        finalImage.gameObject.SetActive(true);
        finalText.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
    }

}
