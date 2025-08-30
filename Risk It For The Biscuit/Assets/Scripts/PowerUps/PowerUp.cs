using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PowerUp : ScriptableObject
{
    public PowerupRarity powerUpType;
    public PowerupActionTime powerUpActionTime;
    public Color background;
    public string powerUpName;
    public string description;
    public float duration;
    public Sprite icon;
    public bool canActivate;

    public int maxUses;


    public void OnEnable()
    {
        if(powerUpType == PowerupRarity.Common)
        {
            background = Color.white;
        }
        else if(powerUpType == PowerupRarity.Epic)
        {
            background = Color.magenta;
        }
        else if(powerUpType == PowerupRarity.Rare)
        {
            background = Color.blue;
        }
        else if(powerUpType == PowerupRarity.Legendary)
        {
            background = Color.red;
        }
        else
        {
            background = Color.gray;
        }
    }

    public void Setup()
    {
        //background = Resources.Load<Sprite>("PowerUps/Backgrounds/" + powerUpType.ToString() + "Background");
    }
    public virtual void Apply(PowerUpContext context)
    {
    }
    
}
