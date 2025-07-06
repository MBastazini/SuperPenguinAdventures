using UnityEngine;


public enum GameState {
    Gameplay,
    Paused
}


public class GameStateManager
{
    private static GameStateManager _instance;
    public static GameStateManager Instance
    { 
        get 
        { 
            if(_instance == null)
                _instance = new GameStateManager();
            return _instance;
        } 
    }

    public GameState CurrentGameState { get; private set; }

    public delegate void GameStateChangeHandler(GameState newGameState);
    public event GameStateChangeHandler OnGameStateChanged;

    public void SetState(GameState newState)
    {
        if (newState == CurrentGameState)
        {
            return;
        }
        CurrentGameState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    private void Awake()
    {
        SetState(GameState.Gameplay);
    }

    private GameStateManager()
    {

    }

}
