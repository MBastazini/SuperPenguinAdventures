using UnityEngine;
using System.Collections;

public class ItemDropper : MonoBehaviour
{
    [SerializeField] private GameObject[] possibleDrops;
    [SerializeField] private float dropChance = 0.3f;

    public void TryDropItem(GameObject killer, Vector3 deathPosition)
    {
        Debug.Log("Tentando dropar item...");
        if (killer == null || possibleDrops == null || possibleDrops.Length == 0) return;
        Debug.Log("NIGGANIGGANIGGA");

        if (dropChance < 0f || dropChance > 1f)
        {
            Debug.LogWarning("Drop chance must be between 0 and 1. Using default value of 0.3.");
            dropChance = 0.3f;
        }

        if (Random.value <= dropChance)
        {
            int index = Random.Range(0, possibleDrops.Length);
            GameObject item = possibleDrops[index];

            // Agora chamamos a coroutine globalmente
            GlobalCoroutineRunner.Instance.RunCoroutine(SpawnItemWithDelay(item, deathPosition, 0.5f));
        }
    }

    private IEnumerator SpawnItemWithDelay(GameObject item, Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(item, position, Quaternion.identity);
    }

    public void NiggaNigga()
    {
        Debug.Log("NIGGANIGGANIGGA");
    }
}
