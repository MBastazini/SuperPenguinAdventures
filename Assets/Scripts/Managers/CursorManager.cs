using UnityEngine;

public enum CursorType
{
    Default,
    EnemyHover
}

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D enemyHoverCursor;
    [SerializeField] private Vector2 cursorHotspot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Garante que só haja um
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject); // Opcional: mantém ao trocar de cena
    }

    public void SetCursor(CursorType type)
    {
        switch (type)
        {
            case CursorType.Default:
                Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
                break;
            case CursorType.EnemyHover:
                Cursor.SetCursor(enemyHoverCursor, cursorHotspot, CursorMode.Auto);
                break;
        }
    }
}
