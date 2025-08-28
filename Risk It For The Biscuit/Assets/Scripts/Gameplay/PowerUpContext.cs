using System.Collections.Generic;
using UnityEngine;

public class PowerUpContext
{
    public bool Won { get; set; }
    public float Points { get; set; }
    public int Lives { get; set; }
    public char CurChar { get; set; }
    public List<string> Words { get; set; }
}
