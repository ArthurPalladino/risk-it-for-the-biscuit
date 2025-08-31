using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PowerUp : ScriptableObject
{
    public PowerupRarity powerUpRarity;
    public PowerUpType powerUpType;
    public Color background;
    public string powerUpName;
    public string description;
    public float duration;
    public Sprite icon;

    [NonSerialized] 
    public bool alreadyActivate = false;

    public int maxUses;


    public void OnEnable()
    {
        if(powerUpRarity == PowerupRarity.Common)
        {
            background = Color.white;
        }
        else if(powerUpRarity == PowerupRarity.Rare)
        {
            background = Color.blue;
        }
        else
        {
            background = Color.white;
        }
    }
    public virtual void Apply(PowerUpContext context)
    {
        alreadyActivate = true;
    }
    
}
