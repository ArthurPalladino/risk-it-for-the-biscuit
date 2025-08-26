using UnityEditor;
using UnityEngine;

public class PowerUp : ScriptableObject
{
    public PowerupRarity powerUpType;
    public Sprite background;
    public string powerUpName;
    public string description;
    public float duration;
    public Sprite icon;
    public bool canActivate;

    public int maxUses;

    public float dropChance;
    public void Setup()
    {
        background = Resources.Load<Sprite>("PowerUps/Backgrounds/" + powerUpType.ToString() + "Background");
        dropChance = powerUpType switch
        {
            PowerupRarity.Common => 95f,
            PowerupRarity.Rare => 15f,
            PowerupRarity.Epic => 4f,
            PowerupRarity.Legendary => 1f,
            _ => 0f
        };
    }
    public virtual void Action()
    {

    }
    
}
