using UnityEngine;


[CreateAssetMenu(menuName = "PowerUps/RiskItToWin")]
public class RiskItToWin : PowerUp
{
    //Após pegar esse PowerUp, o jogador terá permanentemente uma vida menos, porém irá ganhar 1.5x mais pontos.
    public override void Apply(PowerUpContext context)
    {
      context.Lives -= 1;
      context.PointsToSumBeforePowerUps += context.PointsToSumAfterPowerUps * 1.5f;
    }
}