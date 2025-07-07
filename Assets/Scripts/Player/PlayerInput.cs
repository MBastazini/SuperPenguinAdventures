using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent<Vector2> OnMovementInput;

    [SerializeField] GameObject pauseMenu;

    public UnityEvent OnAttackStarted;
    public UnityEvent OnAttackPerformed; // clique rápido
    public UnityEvent OnAttackCanceled;  // pressionado (ou apenas solto depois de tempo)
    public UnityEvent OnPause;

    [SerializeField] private InputActionReference movement, attack, pause;
    private float clickTrashhold = 0.1f;

    private float attackStartTime;

    private void OnEnable()
    {
        movement.action.Enable();
        attack.action.Enable();
        pause.action.Enable();

        attack.action.started += HandleAttackStarted;
        attack.action.canceled += HandleAttackCanceled;
        pause.action.performed += HandlePause;
    }

    private void OnDisable()
    {
        movement.action.Disable();
        attack.action.Disable();
        pause.action.Disable();

        attack.action.started -= HandleAttackStarted;
        attack.action.canceled -= HandleAttackCanceled;
        pause.action.performed -= HandlePause;
    }

    private void Update()
    {
        Vector2 input = movement.action.ReadValue<Vector2>();
        OnMovementInput?.Invoke(input);
    }

    private void HandleAttackStarted(InputAction.CallbackContext context)
    {
        OnAttackStarted?.Invoke();
        attackStartTime = Time.time;
    }

    private void HandleAttackCanceled(InputAction.CallbackContext context)
    {
        float attackDuration = Time.time - attackStartTime;

        if (attackDuration < clickTrashhold)
        {
            OnAttackPerformed?.Invoke();
        }
        OnAttackCanceled?.Invoke();
    }

    private void HandlePause(InputAction.CallbackContext context)
    {
        OnPause?.Invoke();
    }
}