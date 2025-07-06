using UnityEngine;

public class HandleGameStateChage : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 savedVelocity;
    private bool isPaused = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (rb == null)
            Debug.Log("Rigid Body não encontrado");

        if (animator == null)
            Debug.Log("Animator não encontrado");

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChange;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChange;
    }

    void OnGameStateChange(GameState newGameState)
    {
        switch (newGameState)
        {
            case GameState.Paused:
                PausePlayer();
                break;

            case GameState.Gameplay:
                ResumePlayer();
                break;
        }
    }

    private void PausePlayer()
    {
        if (isPaused) return;

        savedVelocity = rb.linearVelocity;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // impede reações físicas enquanto pausado
        animator.speed = 0;
        isPaused = true;
    }

    private void ResumePlayer()
    {
        if (!isPaused) return;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = savedVelocity;
        animator.speed = 1;
        isPaused = false;
    }
}
