using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class AttackRotating : MonoBehaviour, IPlayerAttack
{
    #region Plaer Components

    Rigidbody2D rb; // Rigidbody component for physics interactions
    Transform playerTransform; // Transform component for position and rotation

    #endregion

    [Header("Dash Settings")]
    [SerializeField] float dashStartSpeed = 20f; // Speed for the attack dash
    [SerializeField] float dashBuildUp = 2f; //Says how much the dash speed increases over time
    private float dashSpeed;
    [SerializeField] float dashDeAcceleration = 5f;
    [SerializeField] float dashCooldown = 1f; // Cooldown time in seconds
    [SerializeField] private float maxTimeLoad = 2f;

    [Header("Attack Settings")]
    [SerializeField] private int attackDamage = 1; // Damage dealt by the attack dash
    [SerializeField] private float attackBaseTime = 2f; // Duration of the attack dash

    private float elapsedTime = 0f; //keeps track of how long the attack is being charged

    private Vector2 boost = Vector2.zero;
    private bool isAttackCharging = false;
    private bool isAttackOnCooldown = false; // Indicates if the attack is on cooldown

    private PlayerHitbox playerHitbox; // Reference to the PlayerHitbox component for invincibility frames

    #region eventos
    public UnityEvent OnAttackStart;
    public UnityEvent OnAttackEnd;
    #endregion

    //COROUTINES
    private Coroutine attackChargeCoroutine; //Garante que o jogador segurou o mouse, e não clicou apenas
    private Coroutine attackCooldown;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D não encontrado no objeto " + gameObject.name);
        }
        playerTransform = GetComponent<Transform>();
        if (playerTransform == null)
        {
            Debug.LogError("Transform não encontrado no objeto " + gameObject.name);
        }
        playerHitbox = GetComponent<PlayerHitbox>();
        if (playerHitbox == null)
        {
            Debug.LogError("PlayerHitbox não encontrado no objeto " + gameObject.name);
        }

        dashSpeed = dashStartSpeed; // Initialize dashSpeed with the starting speed
    }

    void FixedUpdate()
    {

        if (isAttackCharging)
        {
            if (ElapsedTime1 < maxTimeLoad)
            {
                dashSpeed += Time.fixedDeltaTime * dashBuildUp;
                ElapsedTime1 += Time.fixedDeltaTime; // Increment elapsed time while charging
            }
        }
        else
        {
            // Gradually reduce the boost over time
            if (boost.magnitude > 0)
            {
                PlayerMovment.movmentBoost = boost; // Update the static boost variable for PlayerMovment
                boost = Vector2.Lerp(boost, Vector2.zero, Time.fixedDeltaTime * dashDeAcceleration);
                if (boost.magnitude < 0.1f) // Threshold to stop the boost
                {
                    boost = Vector2.zero;
                }
            }
        } 
    }

    private bool hasAttackEnded = false;

    public float ElapsedTime { get => ElapsedTime1; set => ElapsedTime1 = value; }
    public float ElapsedTime1 { get => elapsedTime; set => elapsedTime = value; }
    public float ElapsedTime2 { get => elapsedTime; set => elapsedTime = value; }

    //This ggaurantees player is charging an attack

    public void AttackStart()
    {
        if (isAttackOnCooldown) { return; }



        hasAttackEnded = false;

        // Cancela coroutine anterior se ainda estiver rodando
        if (attackChargeCoroutine != null)
        {
            StopCoroutine(attackChargeCoroutine);
            attackChargeCoroutine = null;
        }

        attackChargeCoroutine = StartCoroutine(AttackStartingAfterDelay());
    }


    private IEnumerator AttackStartingAfterDelay()
    {
        yield return new WaitForSeconds(0.3f); // Delay before starting the attack
        if (!hasAttackEnded)
        {
            isAttackCharging = true; // Set the attack as charging
            OnAttackStart?.Invoke(); // Trigger the attack start event
        }
    }


    public void AttackEnd()
    {
        hasAttackEnded = true;

        // Cancela a coroutine se ainda estiver esperando
        if (attackChargeCoroutine != null)
        {
            StopCoroutine(attackChargeCoroutine);
            attackChargeCoroutine = null;
        }

        if (isAttackCharging)
        {
            isAttackCharging = false;
            playerHitbox.StartAttackRotatingIFrames();
            OnAttackEnd?.Invoke();

            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            Vector3 mouseWorldPos3d = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0f);

            Vector2 direction = (mouseWorldPos3d - playerTransform.position).normalized;

            boost = direction * dashSpeed;
            dashSpeed = dashStartSpeed;
            ElapsedTime1 = 0f;

            isAttackOnCooldown = true; // Set the attack on cooldown

            if(attackCooldown != null)
            {
                StopCoroutine(attackCooldown);
                attackCooldown = null;
            }
            attackCooldown = StartCoroutine(AttackDashDelay());

        }
    }

    private IEnumerator AttackDashDelay()
    {
        yield return new WaitForSeconds(dashCooldown);
        isAttackOnCooldown = false; // Reset the cooldown after the specified time
    }

    public float getLoadedPercentage()
    {
        return Mathf.Clamp01(elapsedTime / maxTimeLoad);
    }

    public float getAttackTime()
    {
        return 2f + elapsedTime * 1.5f;
    }

    public int getAttackDamage()
    {
        return 3;
    }

    public int getAttackType()
    {
        return 1;
    }
}
