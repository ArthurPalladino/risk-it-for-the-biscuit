using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PowerUp : ScriptableObject
{
    HangmanManager hangmanManager;
    public PowerupRarity powerUpType;
    public PowerupActionTime powerUpActionTime;
    public Sprite background;
    public string powerUpName;
    public string description;
    public float duration;
    public Sprite icon;
    public bool canActivate;

    public int maxUses;


    public void Start()
    {
        hangmanManager = FindFirstObjectByType<HangmanManager>();
        background = Resources.Load<Sprite>("PowerUps/Backgrounds/" + powerUpType.ToString() + "Background");
    }

    public void Setup()
    {
        //background = Resources.Load<Sprite>("PowerUps/Backgrounds/" + powerUpType.ToString() + "Background");
    }
    public virtual void Apply(PowerUpContext context)
    {
    }
    
}
