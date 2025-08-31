using Unity.VisualScripting;
using UnityEngine;


public enum GameState
{
    MainMenu,
    Playing,
    BuyingPowerUp,
    VisualizingPowerUps,
    FinalScreen
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    public static GameState gameState;

    void Start()
    {
        instance = this;
    }

    public void SetState(GameState newState)
    {
        gameState = newState;
    }

    public GameState GetState()
    {
        return gameState;
    }
}
