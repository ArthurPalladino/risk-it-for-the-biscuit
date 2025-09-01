using System.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "PowerUps/Recoup")]
public class Recoup : PowerUp
{
   /// Quando o jogador lançar W, U ou V, chance de 20% recuperar uma vida
   public override void Apply(PowerUpContext context)
   {
        char[] brothers = { 'w', 'u', 'v'};
        if (brothers.Contains(context.CurChar) && context.Words.Find(x => x.Contains(context.CurChar)) != null)
        {
            if (Random.Range(0, 10) <= 2)
            {
                context.player.addHealth();
            }
        }
   
   }
}
