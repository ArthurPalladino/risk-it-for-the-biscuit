using System;
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

    [SerializeField] float rotationDuration = 2;
    PowerUp powerUp;

    public Action<PowerUp> onSelect;


    public void OnClick()
    {
        onSelect?.Invoke(powerUp);
    }

    public PowerUp GetPowerUp()
    {
        return powerUp;
    }

    public void SetPowerUp(PowerUp newPowerUp)
    {
        if (newPowerUp != powerUp)
        {
            powerUp = newPowerUp;
            RefreshCard();
        }
    }

    // public void RefreshCard()
    // {
    //     //ToDo: Fazer animação com DoTween
    //     if (powerUp != null)
    //     {
    //         sprite.sprite = powerUp.icon;
    //         background.sprite = powerUp.background;
    //         title.text = powerUp.powerUpName;
    //         description.text = powerUp.description;
    //     }
    // }

    public void RefreshCard()
    {
        float trueRotationDuration = rotationDuration / 4f;
        var nextBackground = powerUp.background;
        var nextSprite = powerUp.icon;
        var nextTitle = powerUp.powerUpName;
        var nextDescription = powerUp.description;

        transform.DORotate(new Vector3(0, 90, 0), trueRotationDuration, RotateMode.Fast)
        .OnComplete(() =>
        {
            background.sprite = cardBackSprite;
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
                            background.sprite = nextBackground;
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
