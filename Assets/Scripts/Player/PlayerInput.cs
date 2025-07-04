using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent<Vector2> OnMovementInput;

    public UnityEvent OnAttackStarted;
    public UnityEvent OnAttackPerformed; // clique rápido
    public UnityEvent OnAttackCanceled;  // pressionado (ou apenas solto depois de tempo)

    [SerializeField] private InputActionReference movement, attack;
    [SerializeField] private float clickThreshold = 0.05f;

    private Coroutine clickRoutine;
    private int attackCanceledCount = 0;

    private void OnEnable()
    {
        movement.action.Enable();
        attack.action.Enable();

        attack.action.started += HandleAttackStarted;
        attack.action.canceled += HandleAttackCanceled;
    }

    private void OnDisable()
    {
        movement.action.Disable();
        attack.action.Disable();

        attack.action.started -= HandleAttackStarted;
        attack.action.canceled -= HandleAttackCanceled;
    }

    private void Update()
    {
        Vector2 input = movement.action.ReadValue<Vector2>();
        OnMovementInput?.Invoke(input);
    }

    private void HandleAttackStarted(InputAction.CallbackContext context)
    {
        attackCanceledCount = 0;
        if (clickRoutine != null)
            StopCoroutine(clickRoutine);

        clickRoutine = StartCoroutine(CheckClickRoutine());
        OnAttackStarted?.Invoke();
    }

    private void HandleAttackCanceled(InputAction.CallbackContext context)
    {
        attackCanceledCount++;
        
        OnAttackCanceled?.Invoke();
    }

    private IEnumerator CheckClickRoutine()
    {
        yield return new WaitForSeconds(clickThreshold);
        if (attackCanceledCount > 0)
        {
            OnAttackPerformed?.Invoke(); // Clique rápido
        }
    }
}
