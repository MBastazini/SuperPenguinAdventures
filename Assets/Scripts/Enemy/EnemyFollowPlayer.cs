using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    [SerializeField] EnemyScript enemyScript;
    [SerializeField] float speed = 2f;

    Transform player;

    void Start()
    {
        // Procura o jogador pela tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            Vector2 newPosition = Vector2.MoveTowards(
                enemyScript.GetRigidBody().position,
                player.position,
                speed * Time.fixedDeltaTime
            );
            enemyScript.GetRigidBody().MovePosition(newPosition);
        }
    }
}
