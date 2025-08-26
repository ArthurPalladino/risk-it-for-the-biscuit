using System;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] Image sprite;
    [SerializeField] Image background;
    [SerializeField] TMPro.TMP_Text title;
    [SerializeField] TMPro.TMP_Text description;
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

    public void RefreshCard()
    {
        //ToDo: Fazer animação com DoTween
        if (powerUp != null)
        {
            sprite.sprite = powerUp.icon;
            background.sprite = powerUp.background;
            title.text = powerUp.powerUpName;
            description.text = powerUp.description;
        }
    }
}
