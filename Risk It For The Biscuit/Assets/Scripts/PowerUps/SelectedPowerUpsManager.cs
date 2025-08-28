using System.Collections.Generic;
using UnityEngine;

public class SelectedPowerUpsManager : MonoBehaviour
{
    public static SelectedPowerUpsManager Instance;
    void Awake()
    {
        // Singleton pattern
        // Serve pra passar instancia de objetos entre cenas 
        // e garantir que só existe um objeto instanciado,
        // mas como n vamos usar cena só quis usar YEAH
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    List<PowerUp> selectedPowerUps = new List<PowerUp>();

    public List<PowerUp> GetSelectedPowerUps()
    {
        return selectedPowerUps;
    }

    public void AddPowerUp(PowerUp powerUp)
    {
        selectedPowerUps.Add(powerUp);
    }
    public void RemovePowerUp(PowerUp powerUp)
    {
        selectedPowerUps.Remove(powerUp);
    }
    public void ClearPowerUps()
    {
        selectedPowerUps.Clear();
    }

        List<PowerUp> powerUps = new List<PowerUp>();


    public void StartRound(PowerUpContext context)
    {
        var startPowerups = powerUps.FindAll(x => x.powerUpActionTime == PowerupActionTime.AtStart);
        foreach (var powerUp in startPowerups)
        {
            powerUp.Apply(context);
        }
    }

    public void DuringRound(PowerUpContext context)
    {
        var duringPowerups = powerUps.FindAll(x => x.powerUpActionTime == PowerupActionTime.DuringGame);
        foreach (var powerUp in duringPowerups)
        {
            powerUp.Apply(context);
        }
    }

    public void EndRound(PowerUpContext context)
    {
        var endPowerups = powerUps.FindAll(x => x.powerUpActionTime == PowerupActionTime.AtEnd);
        foreach (var powerUp in endPowerups)
        {
            powerUp.Apply(context);
        }
    }

}
