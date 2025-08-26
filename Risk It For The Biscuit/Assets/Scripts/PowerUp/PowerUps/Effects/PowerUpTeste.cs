using UnityEngine;


[CreateAssetMenu(menuName = "PowerUps/TestPowerUp")]
public class PowerUpTeste : PowerUp
{
   public override void Action()
   {
        Debug.Log("TESTE");
   }
}
