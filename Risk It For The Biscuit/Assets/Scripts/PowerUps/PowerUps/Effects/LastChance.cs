using UnityEngine;


[CreateAssetMenu(menuName = "PowerUps/LastChance")]
public class LastChance : PowerUp
{
   /// Quando o jogador está prestes a perder, este power-up é ativado e dando 1 vida a mais.
   public override void Apply(PowerUpContext context)
   {
      if (alreadyActivate && context.player.curLives<=0)
      {
         context.player.addHealth();
         base.Apply(context);
      }
   
   }
}
