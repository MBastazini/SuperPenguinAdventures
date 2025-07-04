using System.Collections.Generic;
using UnityEngine;

public class EnemyHoverManager : MonoBehaviour
{
    private static List<EnemyScript> enemies = new List<EnemyScript>();

    void Update()
    {
        bool hoveringEnemy = false;

        foreach (var enemy in enemies)
        {
            if (enemy != null && enemy.IsMouseOver())
            {
                hoveringEnemy = true;
                break;
            }
        }

        if (hoveringEnemy)
        {
            CursorManager.Instance.SetCursor(CursorType.EnemyHover);

        }
        else
        {
            CursorManager.Instance.SetCursor(CursorType.Default);

        }
    }

    public static void RegisterEnemy(EnemyScript enemy)
    {
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }

    public static void UnregisterEnemy(EnemyScript enemy)
    {
        enemies.Remove(enemy);
    }
}
