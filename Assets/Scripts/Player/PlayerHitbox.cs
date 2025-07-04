using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{

    PlayerSprite playerSprite;
    Health playerHealth;

    private void Awake()
    {
        playerSprite = GetComponent<PlayerSprite>();
        playerHealth = GetComponent<Health>();
    }

    void StartIFrames(float delay)
    {
        playerHealth.TriggerInvincibility(delay);
        playerSprite.onIFrames(delay);
    }


    public void StartAttackDashIFrames()
    {
        StartIFrames(delay: 0.5f);
    }

    public void StartAttackRotatingIFrames()
    {
        StartIFrames(delay: 1f);
    }
}
