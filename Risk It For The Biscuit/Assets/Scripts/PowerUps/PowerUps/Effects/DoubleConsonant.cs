using UnityEngine;


[CreateAssetMenu(menuName = "PowerUps/DoubleConsonant")]
public class DoubleConsonant : PowerUp
{
    //Quando acertar duas consonantes seguidas ganha dobro de pontos, exemplo, palavra TROIA
    //o power up será ativado se o jogador acertar o T e logo em seguida o R
    public override void Apply(PowerUpContext context)
    {
      foreach(string word in context.Words)
      {
            foreach (char c in word)
            {
                int index = word.IndexOf(c);
                if (index > 0 && word[index - 1] == context.lastChar)
                {
                    context.PointsToSumAfterPowerUps += context.PointsToSumBeforePowerUps*2;
                }
                else if (index < word.Length - 1 && word[index + 1] == context.lastChar)
                {
                    context.PointsToSumAfterPowerUps *= context.PointsToSumBeforePowerUps*2;
                }  
         }
      }
   }
}