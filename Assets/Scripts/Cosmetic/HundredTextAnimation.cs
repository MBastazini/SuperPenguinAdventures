using UnityEngine;

public class HundredTextAnimation : MonoBehaviour
{
    [SerializeField] private int loopMaxCount = 2;
    private int loopCount = 0;

    private void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing on the 100Animation GameObject.");
            return;
        }
        rb.linearVelocityY = 0.5f; // Set the upward velocity for the animation
    }
    public void OnAnimationEnd()
    {
        if(loopCount < loopMaxCount)
        {
            loopCount++;
            return;
        }
        Destroy(gameObject);
    }
}
