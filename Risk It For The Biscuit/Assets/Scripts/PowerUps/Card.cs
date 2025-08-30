using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] Image sprite;
    [SerializeField] Image background;
    [SerializeField] TMPro.TMP_Text title;
    [SerializeField] TMPro.TMP_Text description;

    [SerializeField] Sprite cardBackSprite;
    [SerializeField] Sprite cardFrontSprite;

    [SerializeField] float rotationDuration = 2;

    [SerializeField] PowerUpGrid powerUpGrid;

    [SerializeField] Button closeButton;
    PowerUp powerUp;

    Vector3 originalPos;

    public Action<PowerUp> onSelect;

    void Start()
    {
        originalPos = transform.position;
        var buttons = GetComponentsInChildren<Button>();
        closeButton = buttons.FirstOrDefault(b => b.name == "CloseButton");
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(() => {
                GameStateManager.instance.SetState(GameState.Playing);
                CloseCardGrid();
            });
        }

    }
    public void OnClick()
    {
        onSelect?.Invoke(powerUp);
        powerUpGrid.SetPowerUpsInGrid();
    }

    public PowerUp GetPowerUp()
    {
        return powerUp;
    }

    public void SetPowerUp(PowerUp newPowerUp, bool powerUpScreen = true)
    {
        powerUp = newPowerUp;
        if (powerUpScreen)
        {
            RefreshCardPUScreen();
        }
        else
        {
            RefreshCardGrid();
        }

    }

    public void RefreshCardGrid()
    {
        float halfScreenWidth = Screen.width / 2f - transform.localScale.x * 50f;
        float halfScreenHeight = Screen.height / 2f - transform.localScale.y * 50f;
        background.color = powerUp.background;
        sprite.sprite = powerUp.icon;
        title.text = powerUp.powerUpName;
        description.text = powerUp.description;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(new Vector3(halfScreenWidth, halfScreenHeight, 0), 1f).SetEase(Ease.OutQuad));
        seq.Join(transform.DORotate(new Vector3(0, 720, 0), 1f, RotateMode.FastBeyond360));
        seq.Append(transform.DORotate(Vector3.zero, 0.3f));


    }

    void CloseCardGrid()
    {

        float halfScreenWidth = Screen.width / 2f - transform.localScale.x * 50f; 
        float halfScreenHeight = Screen.height / 2f - transform.localScale.y * 50f;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(originalPos, 1f).SetEase(Ease.OutQuad));
        seq.Join(transform.DORotate(new Vector3(0, 720, 0), 1f, RotateMode.FastBeyond360));
        seq.Append(transform.DORotate(Vector3.zero, 0.3f));
    }

    public void RefreshCardPUScreen()
    {
        float trueRotationDuration = rotationDuration / 4f;
        var nextBackground = cardFrontSprite;
        var nextSprite = powerUp.icon;
        var nextTitle = powerUp.powerUpName;
        var nextDescription = powerUp.description;

        transform.DORotate(new Vector3(0, 90, 0), trueRotationDuration, RotateMode.Fast)
        .OnComplete(() =>
        {
            background.sprite = cardBackSprite;
            background.color = Color.white;
            title.gameObject.SetActive(false);
            description.gameObject.SetActive(false);
            sprite.gameObject.SetActive(false);
            sprite.sprite = nextSprite;
            title.text = nextTitle;
            description.text = nextDescription;
            transform.DORotate(new Vector3(0, 180, 0), trueRotationDuration, RotateMode.Fast)
                .OnComplete(() =>
                {
                    transform.DORotate(new Vector3(0, 270, 0), trueRotationDuration, RotateMode.Fast)
                        .OnComplete(() =>
                        {
                            background.sprite = cardFrontSprite;
                            background.color = powerUp.background;
                            title.gameObject.SetActive(true);
                            description.gameObject.SetActive(true);
                            sprite.gameObject.SetActive(true);
                            transform.DORotate(new Vector3(0, 360, 0), trueRotationDuration, RotateMode.Fast)
                            .OnComplete(() =>
                            {
                                transform.rotation = Quaternion.Euler(0, 0, 0);
                            });
                        });
                });
        });
    }
}
