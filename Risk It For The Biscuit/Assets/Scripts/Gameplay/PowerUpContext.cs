using System.Collections.Generic;
using UnityEngine;

public class PowerUpContext
{
    public bool Won { get; set; }
    public bool Lose { get; set; }
    public float Points { get; set; }
    public int Lives { get; set; }
    public float PointsToSumBeforePowerUps { get; set; }
    public float PointsToSumAfterPowerUps { get; set; }
    public char lastChar { get; set; }
    public char CurChar { get; set; }
    public int correctChars { get; set; }
    public List<string> Words { get; set; }
}
