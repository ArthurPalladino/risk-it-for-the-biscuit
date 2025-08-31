using System.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "PowerUps/DoubleVogals")]
public class DoubleVogals : PowerUp
{
    //Quando acertar duas vogais seguidas ganha dobro de pontos, exemplo, palavra TROIA
    //o power up será ativado se o jogador acertar o T e logo em seguida o R
    public override void Apply(PowerUpContext context)
    {
        char[] vogais = { 'a', 'e', 'i', 'o', 'u' };

        foreach (string word in context.Words)
        {
            for (int i = 0; i < word.Length; i++)
            {
                char c = word[i];
                if (c == context.CurChar)
                {
                    bool lastCharIsVogal = vogais.Contains(context.LastChar);
                    bool curCharIsVogal = vogais.Contains(c);

                    if (lastCharIsVogal && curCharIsVogal)
                    {
                        if ((i > 0 && word[i - 1] == context.LastChar) ||
                            (i < word.Length - 1 && word[i + 1] == context.LastChar))
                        {
                            Debug.Log(context.player.baseMultipl * 1.5);
                            context.player.actualMuitpl += context.player.baseMultipl * 1.5;
                        }
                    }
                }
            }
        }
    }
}