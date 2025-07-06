using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour
{
    [SerializeField] private GetScriptsOfClass getHealthPlayerScripts;
    [SerializeField] private Image healthBarImage; // Assuming you have a UI Image for the health bar
    [SerializeField] private TextMeshProUGUI enemyKilledText; // Text to display the number of enemies killed

    [SerializeField] private int enemyKilledCount = 0; // Counter for enemies killed

    private Health healthPlayerScript;

    void Awake()
    {
        if (getHealthPlayerScripts == null)
        {
            Debug.LogError("GetScriptsOfClass is not assigned.");
            return;
        }
    }

    public void UpdateHealthBarOnMenu()
    {
        if(healthPlayerScript == null)
        {
            Debug.LogError("Health script is not initialized.");
            return;
        }
        int[] playerCurrentHealth = healthPlayerScript.GetHealthAndMaxHealth();

        if (healthBarImage != null)
        {
            // Calculate the fill amount based on current health and max health
            float fillAmount = (float)playerCurrentHealth[0] / playerCurrentHealth[1];
            healthBarImage.fillAmount = fillAmount;
        }
        else
        {
            Debug.LogWarning("Health bar image is not assigned.");
        }
    }

    public void SetHealthPlayerScripts(Component[] scripts)
    {
        Health healthScript = scripts[0] as Health;
        if (healthScript == null)
        {
            Debug.LogError("Health script array is null.");
            return;
        }
        healthPlayerScript = healthScript;
    }


    public void OnEnemyKill()
    {
        enemyKilledCount++;
        if (enemyKilledText != null)
        {
            // Correcting the error by using enemyKilledCount.ToString() instead of "string"
            enemyKilledText.text = (enemyKilledCount * 100).ToString();
        }
        else
        {
            Debug.LogWarning("Enemy killed text is not assigned.");
        }
    }

}
