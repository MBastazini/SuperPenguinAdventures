using UnityEngine;
using System.Collections;

public class GlobalCoroutineRunner : MonoBehaviour
{
    public static GlobalCoroutineRunner Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public void RunCoroutine(System.Action function, float delay)
    {
        StartCoroutine(RunDelayedRespectingPause(function, delay));
    }

    private IEnumerator RunDelayedRespectingPause(System.Action function, float delay)
    {
        float elapsed = 0f;

        while (elapsed < delay)
        {
            // Espera o jogo estar em Gameplay para contar o tempo
            if (GameStateManager.Instance.CurrentGameState == GameState.Gameplay)
            {
                elapsed += Time.unscaledDeltaTime; // unscaledDeltaTime ignora o Time.timeScale
            }

            yield return null;
        }

        function?.Invoke();
    }
}
