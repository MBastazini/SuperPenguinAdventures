using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GetAllScripts : MonoBehaviour
{
    private MonoBehaviour[] m_Scripts;

    private void Awake()
    {
        m_Scripts = GetComponents<MonoBehaviour>();
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;

        // Remove os elementos do tipo PlayerInput
        m_Scripts = m_Scripts.Where(script => script is not PlayerInput).ToArray();
    }


    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    void OnGameStateChanged(GameState newGameState)
    {
        foreach (var script in m_Scripts)
        {
            script.enabled = newGameState == GameState.Gameplay;
        }
    }
}
