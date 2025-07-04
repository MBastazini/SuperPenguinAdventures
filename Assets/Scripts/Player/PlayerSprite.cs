using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    SpriteRenderer playerSprite;

    private void Awake()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        if (playerSprite == null)
        {
            Debug.LogError("SpriteRenderer nao encontrado no objeto " + gameObject.name);
        }
    }
    
    public void onIFrames(float delay)
    {
        playerSprite.color = Color.yellow;
        Invoke("ResetColor", delay);
    }


    public void OnHit()
    {
        playerSprite.color = Color.red;
        Invoke("ResetColor", 0.1f);
    }

    public void OnHeal()
    {
        playerSprite.color = Color.green;
        Invoke("ResetColor", 0.3f);
    }
    void ResetColor()
    {
        playerSprite.color = Color.white;
    }

}
