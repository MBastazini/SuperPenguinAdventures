using System.Collections;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [Header("Refer�ncias")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private BoxCollider2D mapBoundaries;

    [Header("Configura��es")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnRadius = 50f; // Grande o suficiente para cobrir o mapa

    private void Start()
    {
        StartCoroutine(SpawnEnemiesRoutine());
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            TrySpawnEnemy();
        }
    }

    private void TrySpawnEnemy()
    {
        Vector2 spawnPosition;

        // Limite de tentativas para evitar loop infinito
        int maxAttempts = 20;
        int attempts = 0;

        do
        {
            spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            attempts++;
        }
        while (!IsInsideMap(spawnPosition) && attempts < maxAttempts);

        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("N�o foi poss�vel encontrar uma posi��o v�lida para spawn.");
            return;
        }

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        EnemyScript enemyScript = newEnemy.GetComponent<EnemyScript>();
        if (enemyScript != null)
        {
            enemyScript.SetPlayer(player);
        }
        else
        {
            Debug.LogWarning("EnemyPrefab n�o tem EnemyScript.");
        }
    }

    private bool IsInsideMap(Vector2 point)
    {
        return mapBoundaries.OverlapPoint(point);
    }
}
