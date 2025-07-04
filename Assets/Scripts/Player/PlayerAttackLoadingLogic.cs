using System.Collections;
using UnityEngine;

public class PlayerAttackLoadingLogic : MonoBehaviour
{
    private SpriteRenderer swordSpriteRenderer;
    [SerializeField] private Animator swordLoadingAnimator;

    private bool isLoading = false;

    private void Awake()
    {
        swordLoadingAnimator = GetComponent<Animator>();
        if (swordLoadingAnimator == null)
        {
            Debug.LogError("Animator não encontrado no objeto da espada: " + gameObject.name);
        }
        swordSpriteRenderer = GetComponent<SpriteRenderer>();
        if (swordSpriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer não encontrado no objeto da espada: " + gameObject.name);
        }
    }


    public void StartLoading()
    {
        if (isLoading) return;
        isLoading = true;
        swordSpriteRenderer.enabled = true; // Ativa o SpriteRenderer para mostrar a espada durante o carregamento
        print("OIEEEEE");
        swordLoadingAnimator.SetBool("IsLoading", true);
        swordLoadingAnimator.SetTrigger("Attack");
    }

    public void EndLoading()
    {
        if (!isLoading) return;
        isLoading = false;
        //swordLoadingAnimator.SetBool("IsLoading", false);
        swordSpriteRenderer.enabled = false; // Desativa o SpriteRenderer após o carregamento
    }
}
