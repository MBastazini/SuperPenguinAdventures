using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyGenerator : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private BoxCollider2D mapBoundaries;

    [Header("Configurações")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnRadius = 50f; // Grande o suficiente para cobrir o mapa

    [Header("Bonus")]
    [SerializeField] private GameObject bonusPrefab;


    [SerializeField] private UnityEvent<GameObject, Vector3> onEnemyDeath;


    public bool canSpawnEnemys = true;


    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChange;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChange;
    }

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
        if(!canSpawnEnemys)
        {
            return;
        }
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
            Debug.LogWarning("Não foi possível encontrar uma posição válida para spawn.");
            return;
        }

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        EnemyScript enemyScript = newEnemy.GetComponent<EnemyScript>();
        if (enemyScript != null)
        {
            enemyScript.SetPlayer(player);
            enemyScript.SetEnemyOnDeathEvent(onEnemyDeath);
        }
        else
        {
            Debug.LogWarning("EnemyPrefab não tem EnemyScript.");
        }
    }

    private bool IsInsideMap(Vector2 point)
    {
        return mapBoundaries.OverlapPoint(point);
    }

    public void SpawnPrefabWhenEnemyKilled(GameObject _, Vector3 position)
    {
        if (bonusPrefab == null)
        {
            Debug.LogWarning("bonusPrefab não está atribuído no EnemyGenerator.");
            return;
        }

        //JOga um pouco mais pra cima
        Vector3 adjustedPosition = position + new Vector3(0, 2f, 0);

        Instantiate(bonusPrefab, adjustedPosition, Quaternion.identity);
    }

    void OnGameStateChange(GameState newGameState)
    {
        canSpawnEnemys = newGameState == GameState.Gameplay;
    }
}
