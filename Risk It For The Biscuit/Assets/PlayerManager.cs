using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    const int MAX_LIVES = 5;
    public int curLives = MAX_LIVES;
    public int maxGameplayLives = MAX_LIVES;

    [SerializeField] public ParticleSystem Blood;

    [SerializeField] public List<BodyPart> BodyParts;

  
    public void restore()
    {
        curLives = MAX_LIVES;

        foreach (BodyPart part in BodyParts)
        {
            part.Restore();
        }
    }

    public void addHealth()
    {
        curLives += 1;
    }

    public void removeHealth()
    {
        var part = BodyParts[BodyParts.Count - (curLives + 1)];

        //if ((BodyParts.Count - (curLives + 1) - 1) > 0)
        //{
        //    var pastPart = BodyParts[BodyParts.Count - (curLives + 1) - 1];
        //    if (pastPart != null)
        //    {
        //        pastPart.Dismiss();
        //    }
        //}

        part.Cut();
        
        curLives -= 1;
    }
  
}
