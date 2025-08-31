using System.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "PowerUps/IHateTheseLetters")]
public class IHateTheseLetters : PowerUp
{
   /// Ganha 1.2x mais pontos caaso as letras sejam XYW
   public override void Apply(PowerUpContext context)
   {
      char[] letras = { 'X', 'Y', 'Z' }; 
      if (letras.Contains(context.CurChar))
      {
         context.player.actualMuitpl += context.player.baseMultipl * 1.2;
      }
   }
}
