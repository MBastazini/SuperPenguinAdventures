using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
public class PlayerMovment : MonoBehaviour
{
    
    [Header("Player Components")]
    [SerializeField] Rigidbody2D rb;
    Animator animator;
    Transform playerTransform;

    [Header("Player settings")]
    [SerializeField] float speed = 5f;

    

    //ANIMATION SETTINGS
    private Vector2 lastMoveDirection;
    //private bool facingLeft = true;


    private Vector2 playerMovment = Vector2.zero;
    private Vector2 newMovment = Vector2.zero;

    public static Vector2 movmentBoost = Vector2.zero;
    public static bool canMove = true;

    public Vector2 NewMovment { get => newMovment; set => newMovment = value; }

    void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("Rigidbody2D não encontrado no objeto " + gameObject.name);
            }
        }
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator não encontrado no objeto " + gameObject.name);
        }
        playerTransform = GetComponent<Transform>();
        if (playerTransform == null)
        {
            Debug.LogError("Transform não encontrado no objeto " + gameObject.name);
        }
    }

    void Start()
    {
            
    }

    void Update()
    {
        HandleInputs();
        Animate();

        /*if (newMovment.x < 0 && !facingLeft)
        {
            Flip();
        }
        else if (newMovment.x > 0 && facingLeft)
        {
            Flip();
        }*/
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        rb.linearVelocity = (playerMovment * speed) + movmentBoost;
    }

    

    void HandleInputs()
    {
        if ((newMovment.x == 0 && newMovment.y == 0) && (playerMovment.x != 0 || playerMovment.y != 0))
        {
       
            lastMoveDirection = playerMovment;
            //print("Last Move Direction: " + lastMoveDirection);
        }

        playerMovment = newMovment;
    }

    void Animate()
    {
        animator.SetFloat("moveX", playerMovment.x);
        animator.SetFloat("moveY", playerMovment.y);
        animator.SetFloat("lastMoveX", lastMoveDirection.x);
        animator.SetFloat("lastMoveY", lastMoveDirection.y);
        animator.SetFloat("moveMagnitude", playerMovment.magnitude);
    }

    /*void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingLeft = !facingLeft;
    }*/

}
