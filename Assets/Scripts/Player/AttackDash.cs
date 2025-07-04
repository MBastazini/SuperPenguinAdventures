using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using UnityEngine.Events;
public class AttackDash : MonoBehaviour, IPlayerAttack
{

    #region Plaer Components

    Rigidbody2D rb; // Rigidbody component for physics interactions
    Transform playerTransform; // Transform component for position and rotation

    #endregion
    //ATTACK DASH SETTINGS
    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 20f; // Speed for the attack dash
    [SerializeField] float dashDeAcceleration = 5f;
    [SerializeField] float dashCooldown = 1f; // Cooldown time in seconds

    [Header("Attack Settings")]
    [SerializeField] private int attackDamage = 1; // Damage dealt by the attack dash
    [SerializeField] private float attackTime = 0.4f; // Duration of the attack dash



    private Vector2 boost = Vector2.zero;
    private bool isAttackDashOnCooldown = false;

    private PlayerHitbox playerHitbox; // Reference to the PlayerHitbox component for invincibility frames

    #region eventos
    public UnityEvent OnAttackInitiated;
    #endregion


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
    }

    void FixedUpdate()
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

    public void Attack()
    {
        if (isAttackDashOnCooldown)
        {
            //Debug.Log("Attack Dash is on cooldown.");
            return;
        }

        playerHitbox.StartAttackDashIFrames(); // Start invincibility frames for the attack dash

        isAttackDashOnCooldown = true;
        // Runs on mouse click
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        Vector3 mouseWorldPos3d = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0f);

        //get the vector from player to mouse position
        Vector2 direction = (mouseWorldPos3d - playerTransform.position).normalized;

        // Accelerate the player in the direction of the mouse position

        boost = direction * dashSpeed;

        //print("Attack!");

        // Trigger the attack dash initiated event
        OnAttackInitiated?.Invoke(); // O '?' verifica se há assinantes antes de invocar

        if(attackCooldown != null)
        {
            StopCoroutine(attackCooldown);
            attackCooldown = null;
        }

        attackCooldown = StartCoroutine(AttackDashDelay());
    }

    private IEnumerator AttackDashDelay()
    {
        yield return new WaitForSeconds(dashCooldown);
        isAttackDashOnCooldown = false;
        //Debug.Log("Attack Dash cooldown ended.");
    }

    public float getLoadedPercentage()
    {
        return 1f;
    }

    public float getAttackTime()
    {
        return attackTime;
    }

    public int getAttackDamage()
    {
        return attackDamage;
    }

    public int getAttackType()
    {
        return 0;
    }
}
