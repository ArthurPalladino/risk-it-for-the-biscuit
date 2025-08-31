using UnityEngine;


[CreateAssetMenu(menuName = "PowerUps/RiskItToWin")]
public class RiskItToWin : PowerUp
{
  //Após pegar esse PowerUp, o jogador terá permanentemente uma vida menos, porém irá ganhar 1.5x mais pontos.
  public override void Apply(PowerUpContext context)
  {
    Debug.Log("APPLY");
    Debug.Log(alreadyActivate);
    if (!alreadyActivate)
    {

      context.player.maxGameplayLives -= 1;
      context.player.baseMultipl *= 1.5f;
      base.Apply(context);
    }
  }
}