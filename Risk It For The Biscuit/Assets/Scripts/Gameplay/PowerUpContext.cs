using System.Collections.Generic;
using UnityEngine;

public class PowerUpContext
{
    public PlayerManager player;
    public bool Won { get; set; }
    public bool Lose { get; set; }
    public double Points { get; set; }
    public int RightLetters { get; set; }
    public float PointsToSumBeforePowerUps { get; set; }
    public float PointsToSumAfterPowerUps { get; set; }
    public char LastChar { get; set; }
    public char CurChar { get; set; }
    public int correctChars { get; set; }
    public List<string> Words { get; set; }
}
