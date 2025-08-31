using UnityEngine;


[CreateAssetMenu(menuName = "PowerUps/OneLetterWord")]
public class OneLetterWord : PowerUp
{
   /// Quando o jogador acerta uma letra que está em 3 lugares ganha 1.5x mais pontos
   public override void Apply(PowerUpContext context)
   {
      if (context.RightLetters >= 3)
      {
         context.player.actualMuitpl += context.player.baseMultipl * 1.5;
      }
   }
}
