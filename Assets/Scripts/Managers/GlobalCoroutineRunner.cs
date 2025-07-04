using UnityEngine;
using System.Collections;

public class GlobalCoroutineRunner : MonoBehaviour
{
    public static GlobalCoroutineRunner Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Opcional: persiste entre cenas
    }

    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
