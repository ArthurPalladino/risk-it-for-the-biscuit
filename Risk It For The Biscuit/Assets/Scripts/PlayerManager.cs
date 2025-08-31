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

    public double points;

    public double baseMultipl = 4;

    public double actualMuitpl;


    [SerializeField] public ParticleSystem Blood;

    [SerializeField] public List<BodyPart> BodyParts;

    [SerializeField] public TMP_Text LivesText;


    public void RestoreGameplay()
    {
        curLives = MAX_LIVES;
        maxGameplayLives = MAX_LIVES;
        points = 0;
        updadeLivesText();
        RestoreBodysByLife();
    }
    void RestoreBodysByLife()
    {
        for (int i = 0; i < BodyParts.Count; i++)
        {
            var part = BodyParts[i];
            part.Restore();
        }

        var count = MAX_LIVES - maxGameplayLives;
        if (count >= 1)
        {
            for (int i = 0; i < MAX_LIVES - maxGameplayLives; i++)
            {
                removeHealth(false);
            }

        }
    }
    public void restoreRound()
    {
        curLives = maxGameplayLives;
        points = 0;
        RestoreBodysByLife();
        updadeLivesText();
    }

    public void addHealth()
    {
        curLives += 1;
        var part = BodyParts[BodyParts.Count - curLives];
        part.Restore();
        updadeLivesText();
    }
    public void removeHealth(bool removeLive = true)
    {
        var part = BodyParts[BodyParts.Count - (curLives + 1)];

        part.Cut();
        if (removeLive)
        {
            curLives -= 1;
        }
        updadeLivesText();
    }

    private void updadeLivesText()
    {
        LivesText.text = curLives + "/" + maxGameplayLives;
    }

}
