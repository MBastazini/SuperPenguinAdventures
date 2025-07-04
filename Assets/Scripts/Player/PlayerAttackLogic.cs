using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttackLogic : MonoBehaviour
{
    private SpriteRenderer swordSpriteRenderer;
    [SerializeField] private Animator swordAnimator;
    private IPlayerAttack currentAttackScript;

    [SerializeField] private int AttackType;
    public LayerMask damageableLayers;
    public float radius;

    void Awake()
    {

        swordSpriteRenderer = GetComponent<SpriteRenderer>();
        if (swordSpriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer não encontrado no objeto da espada: " + gameObject.name);
        }

        IPlayerAttack[] playerAttackScripts = transform
        .GetComponentsInParent<MonoBehaviour>(true)
        .OfType<IPlayerAttack>()
        .ToArray();

        if (playerAttackScripts.Length == 0)
        {
            Debug.LogError("Nenhum script de ataque encontrado nos objetos pais.");
            return;
        }

        var filteredAttacks = new List<IPlayerAttack>();
        foreach (var script in playerAttackScripts)
        {
            if (script.getAttackType() == AttackType)
            {
                filteredAttacks.Add(script);
            }
        }

        if (filteredAttacks.Count == 0)
        {
            Debug.LogError("Nenhum script com o AttackType: " + AttackType);
            return;
        }
        if (filteredAttacks.Count > 1)
        {
            Debug.LogWarning("Mais de um script com o mesmo AttackType encontrado.");
        }

        currentAttackScript = filteredAttacks[0];


    }

    void OnEnable()
    {

        if (swordSpriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer da espada não está definido.");
            return;
        }
        swordSpriteRenderer.enabled = false; // Desativa o SpriteRenderer inicialmente
        //print(gameObject.name + " - SpriteRenderer da espada desativado. (1!!!)");
    }

    public void PlaySwordAttackAnimation()
    {
        swordAnimator.SetBool("AttackEnded", false);
        swordAnimator.SetBool("IsLoading", true); // Certifique-se de que a animação de carregamento não esteja ativa
        // Ativa o SpriteRenderer para mostrar a espada durante o ataque
        if (swordSpriteRenderer != null)
        {
            swordSpriteRenderer.enabled = true;
            //print(gameObject.name + " - SpriteRenderer da espada ATIVADO!!!!!.");
        }
        else
        {
            Debug.LogError("SpriteRenderer da espada não está definido.");
        }
        if (swordAnimator != null)
        {
            // Supondo que você tenha um trigger no seu Animator chamado "Attack"
            swordAnimator.SetTrigger("Attack");
            StartCoroutine(OnAttackEnd());
        }
    }

    private IEnumerator OnAttackEnd()
    {
        yield return new WaitForSeconds(currentAttackScript.getAttackTime());
        swordAnimator.SetBool("AttackEnded", true);
        if (swordSpriteRenderer != null)
        {
            swordSpriteRenderer.enabled = false;
            //print(gameObject.name + " - SpriteRenderer da espada desativado. (2!!!)");
        }
        else
        {
            Debug.LogError("SpriteRenderer da espada não está definido.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    public void DetectColliders()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, damageableLayers);

        foreach (Collider2D collider in colliders)
        {
            Health health;
            if (health = collider.GetComponent<Health>())
            {
                health.GetHit(currentAttackScript.getAttackDamage(), transform.parent.gameObject); // Altera o valor de dano conforme necessário
            }
        }
    }
}
