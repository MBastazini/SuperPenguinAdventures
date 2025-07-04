using UnityEngine;

public class AppleScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitbox"))
        {
            Health playerHealth = collision.transform.parent.gameObject.GetComponent<Health>();
            PlayerSprite playerSprite = collision.transform.parent.gameObject.GetComponent<PlayerSprite>();
            if (playerHealth != null)
            {
                playerHealth.Heal(3);
                playerSprite.OnHeal(); // Call the OnHeal method to change the sprite color
            }
            Destroy(gameObject); // Destroy the apple after healing
        }
    }
}
