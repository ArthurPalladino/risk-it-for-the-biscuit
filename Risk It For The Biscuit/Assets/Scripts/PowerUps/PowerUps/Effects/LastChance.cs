using UnityEngine;


[CreateAssetMenu(menuName = "PowerUps/LastChance")]
public class LastChance : PowerUp
{
   /// Quando o jogador está prestes a perder, este power-up é ativado e dando 1 vida a mais.
   public override void Apply(PowerUpContext context)
   {
      if(context.Lose)
      {
         context.Lives += 1;
      }
   }
}
