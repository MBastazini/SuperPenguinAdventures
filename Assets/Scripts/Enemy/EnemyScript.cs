using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyScript : MonoBehaviour
{
    #region atributos_gerais

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private GameObject player;

    [SerializeField] private Health enemyHealth;

    #endregion


    void OnEnable()
    {
        EnemyHoverManager.RegisterEnemy(this);
    }

    void OnDisable()
    {
        EnemyHoverManager.UnregisterEnemy(this);
    }

    public void SetPlayer(GameObject playerRef)
    {
        player = playerRef;
    }

    public void SetEnemyOnDeathEvent(UnityEngine.Events.UnityEvent<GameObject, Vector3> onDeathEvent)
    {
        if(enemyHealth == null)
        {
            Debug.LogError("Health component not found on " + gameObject.name);
            return;
        }
        enemyHealth.SetOnDeathEvent(onDeathEvent);
    }


    public bool IsMouseOver()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        return boxCollider.OverlapPoint(mouseWorldPos);
    }

    public Rigidbody2D GetRigidBody()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("Rigidbody2D não encontrado no objeto " + gameObject.name);
                return null;
            }
        }
        return rb;
    }

    public void onHit()
    {
        //Change sprite color to indicate hit

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            Invoke("ResetColor", 0.1f); // Reset color after 0.1 seconds
        }
        else
        {
            Debug.LogError("SpriteRenderer não encontrado no objeto " + gameObject.name);
        }
    }

    private void ResetColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white; // Reset to original color
        }
        else
        {
            Debug.LogError("SpriteRenderer não encontrado no objeto " + gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //print("Colidiu????" + other.name);
        if (other.CompareTag("PlayerHitbox"))
        {
            Health playerHealth = other.transform.parent.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.GetHit(1, gameObject);
            }
        }
    }

}
