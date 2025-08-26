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
    int refreshTimes = 1111111;

    Action AfterClose;
    Vector3 originalPos;
    void Start()
    {
        refreshButton.onClick.AddListener(Refresh);
        refreshText = refreshButton.GetComponentInChildren<TextMeshProUGUI>();
        foreach (var card in cards)
        {
            card.onSelect += OnSelect;
        }
        UpdateText();
        SetupCards();
    }
    void OnEnable()
    {
        if (originalPos == Vector3.zero)
        {
            originalPos = panelTransform.position;
        }
        SetupCards();
        panelTransform.position = originalPos + Vector3.up * 10;
        panelTransform.DOMove(originalPos, 1);
    }
    public void SetupCards()
    {
        foreach (var card in cards)
        {
            PowerUp powerUp = GetRandomPowerUp();
            powerUp.Setup();
            card.SetPowerUp(powerUp);
        }
    }

    void OnSelect(PowerUp powerUp)
    {
        SelectedPowerUp = powerUp;
        SelectedPowerUpsManager.Instance.AddPowerUp(SelectedPowerUp);
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
        Vector3 offScreenPosition = transform.position + Vector3.down * 10f;

        panelTransform.DOMove(offScreenPosition, 1).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
        AfterClose?.Invoke();
    }

    PowerUp GetRandomPowerUp(int curLevel = 1)
    {

        List<PowerUp> availablePowerUps = allPowerUps.Where(p => !cards.Any(c => c.GetPowerUp() == p)).ToList();
        float rand = UnityEngine.Random.value * 100f;
        List<PowerUp> selectedList;
        PowerupRarity choosedRarity;
        if (rand <= (int)PowerupRarity.Legendary)
            choosedRarity = PowerupRarity.Legendary;
        else if (rand <= (int)PowerupRarity.Rare)
            choosedRarity = PowerupRarity.Rare;
        else if (rand <= (int)PowerupRarity.Epic)
            choosedRarity = PowerupRarity.Epic;
        else
            choosedRarity = PowerupRarity.Common;

        selectedList = availablePowerUps.Where(p => p.powerUpType == choosedRarity).ToList();
        return selectedList[UnityEngine.Random.Range(0, selectedList.Count)];
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
