using UnityEngine;


[CreateAssetMenu(menuName = "PowerUps/RoundPoints")]
public class RoundPoints : PowerUp
{
    //Após pegar esse PowerUp não será mais ter pontos terminados em 5, caso vc tenha 92 pontos você vai ter 90 pontos, caso tenha 96 pontos você vai ter 100 pontos.
    public override void Apply(PowerUpContext context)
    {
        //garantir que esse seja o ultimo powerup a rodar
        context.PointsToSumAfterPowerUps = Mathf.Round(context.PointsToSumAfterPowerUps / 10) * 10;
    }
}