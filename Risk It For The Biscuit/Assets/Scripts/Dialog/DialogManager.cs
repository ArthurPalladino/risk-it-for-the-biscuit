using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogText;

    [SerializeField] Image sprite;

    [SerializeField] GameObject panel;
    [SerializeField] float typingSpeed = 0.04f;

    [SerializeField] AudioClip dialogSound;

    Audio dialogAudio;

    public static DialogSystem Instance;

    string curText;
    bool isTalking;
    void Start()
    {
        dialogAudio = new Audio() { Clip = dialogSound, Loop = true };

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !isTalking)
        {
            ShowDialog("TESTEEEEEEEEEEEEETESTEEEEEEEEEEEEETESTEEEEEEEEEEEEETESTEEEEEEEEEEEEETESTEEEEEEEEEEEEETESTEEEEEEEEEEEEETESTEEEEEEEEEEEEETESTEEEEEEEEEEEEETESTEEEEEEEEEEEEE");
        }

        if ((Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.KeypadEnter)) && isTalking)
        {
            FinishDialog();
        }
    }
    public static void ShowDialog(string text, Sprite newSprite = null)
    {
        if (Instance != null && !Instance.isTalking)
        {
            Instance.StartCoroutine(Instance.TypeDialog(text));
            Instance.isTalking = true;
            Instance.panel.SetActive(true);
            Instance.sprite.gameObject.SetActive(true);
            if (newSprite != null)
            {
                Instance.sprite.sprite = newSprite;
            }
        }
    }
    IEnumerator TypeDialog(string text)
    {
        AudioManager.Instance.Resume(dialogAudio);
        dialogText.text = "";
        curText = text;
        foreach (char c in text.ToCharArray())
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        AudioManager.Instance.Pause(dialogAudio);
        isTalking = false;
        //panel.SetActive(false);
    }

    void FinishDialog()
    {
        StopAllCoroutines();
        dialogText.text = curText;
        AudioManager.Instance.Pause(dialogAudio);
        isTalking = false;
    }
}
