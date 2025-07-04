using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent<Vector2> OnMovementInput;

    // Eventos separados
    public UnityEvent OnAttackStarted;     // Quando mouse � pressionado
    public UnityEvent OnAttackPerformed;   // S� se for um clique r�pido
    public UnityEvent OnAttackCanceled;    // Quando mouse � solto (pressionado ou n�o)

    [SerializeField] private InputActionReference movement, attack;

    [SerializeField] private float clickThreshold = 0.1f; // M�ximo de tempo para ser considerado um clique

    private float attackPressStartTime;
    private bool waitingToCheckClick = false;

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
        attackPressStartTime = Time.time;
        waitingToCheckClick = true;
        OnAttackStarted?.Invoke();
    }

    private void HandleAttackCanceled(InputAction.CallbackContext context)
    {
        float heldDuration = Time.time - attackPressStartTime;

        if (waitingToCheckClick && heldDuration < clickThreshold)
        {
            OnAttackPerformed?.Invoke(); // Real click
        }

        waitingToCheckClick = false;
        OnAttackCanceled?.Invoke();
    }
}
