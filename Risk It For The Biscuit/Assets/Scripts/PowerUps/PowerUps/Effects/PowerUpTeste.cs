using UnityEngine;


[CreateAssetMenu(menuName = "PowerUps/TestPowerUp")]
public class PowerUpTeste : PowerUp
{
   public override void Apply(PowerUpContext context)
   {
      context.Points += 10;
   }
}
