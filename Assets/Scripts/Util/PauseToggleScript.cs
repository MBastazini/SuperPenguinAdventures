using UnityEngine;

public class PauseToggleScript : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += TogglePauseMenuEnabled;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= TogglePauseMenuEnabled;
    }
    void TogglePauseMenuEnabled(GameState newGameState)
    {
        pauseMenu.SetActive(newGameState == GameState.Paused);
    }
}
