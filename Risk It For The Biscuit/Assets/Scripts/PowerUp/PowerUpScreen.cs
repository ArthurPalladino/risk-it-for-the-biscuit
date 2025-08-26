using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpScreen : MonoBehaviour
{
    [SerializeField] List<PowerUp> allPowerUps;
    [SerializeField] Card[] cards;
    [SerializeField] Transform panelTransform;

    PowerUp SelectedPowerUp;


    [SerializeField] Button refreshButton;
    TextMeshProUGUI refreshText;
    int refreshTimes = 3;

    Action AfterClose;
    Vector3 originalPos;
    void Start()
    {
        refreshButton.onClick.AddListener(Refresh);
        refreshText = refreshButton.GetComponentInChildren<TextMeshProUGUI>();
        originalPos = panelTransform.position;
        foreach (var card in cards)
        {
            card.onSelect += OnSelect;
        }
        UpdateText();
        SetupCards();
    }
    void OnEnable()
    {
        panelTransform.position = originalPos + Vector3.up * 10;
        panelTransform.DOMove(originalPos, 1);
    }
    public void SetupCards()
    {
        List<PowerUp> availablePowerUps = allPowerUps.OrderBy(x => Guid.NewGuid()).ToList();

        foreach (var card in cards)
        {
            //ToDo: Utilizar o GetRandom e implementar probabilidade de drop por raridade.
            PowerUp powerUp = availablePowerUps[0];
            powerUp.Setup();
            availablePowerUps.RemoveAt(0);
            card.SetPowerUp(powerUp);
        }
    }

    void OnSelect(PowerUp powerUp)
    {
        SelectedPowerUp = powerUp;
        Close();
    }


    public void Refresh()
    {
        if (refreshTimes > 0)
        {
            refreshTimes--;
            UpdateText();
            SetupCards();

        }
        else
        {
            StartCoroutine(BlinkEffect());
        }
    }

    IEnumerator BlinkEffect()
    {
        Color blinkColor = Color.red; 
        float blinkDuration = 0.25f;

        Image image = refreshButton.GetComponent<Image>();
        Color originalColor = Color.white;

        float timer = 0;
        while (timer < blinkDuration)
        {
            image.color = Color.Lerp(originalColor, blinkColor, Mathf.PingPong(timer * 2, 1));
            timer += Time.deltaTime;
            yield return null;
        }
        image.color = originalColor;
    }

    public void Close()
    {
        Debug.Log("Closed PowerUp Screen");
        Vector3 offScreenPosition = transform.position + Vector3.down * 10f;

        panelTransform.DOMove(offScreenPosition, 1);
        AfterClose?.Invoke();
    }

    PowerUp GetRandomPowerUp(int curLevel = 1)
    {

        return allPowerUps[UnityEngine.Random.Range(0, allPowerUps.Count)];
    }

    void UpdateText()
    {
        refreshText.text = refreshTimes.ToString();
    }

    void SetRefreshTimes(int times)
    {
        refreshTimes = times;
    }

}
