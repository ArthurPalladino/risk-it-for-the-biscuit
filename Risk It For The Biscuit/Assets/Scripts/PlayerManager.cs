using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    const int MAX_LIVES = 5;
    public int curLives = MAX_LIVES;
    public int maxGameplayLives = MAX_LIVES;

    public float points;

    [SerializeField] public ParticleSystem Blood;

    [SerializeField] public List<BodyPart> BodyParts;

    [SerializeField] public TMP_Text LivesText;

  
    public void restore()
    {
        curLives = MAX_LIVES;
       

        foreach (BodyPart part in BodyParts)
        {
            part.Restore();
        }
        updadeLivesText();
    }

    public void addHealth()
    {
        curLives += 1;
        updadeLivesText();
    }

    public void removeHealth()
    {
        var part = BodyParts[BodyParts.Count - (curLives + 1)];

        part.Cut();
        
        curLives -= 1;
        updadeLivesText();
    }

    private void updadeLivesText()
    {
        LivesText.text = curLives + "/" + MAX_LIVES;
    }

}
