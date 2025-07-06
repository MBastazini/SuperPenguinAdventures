using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Health : MonoBehaviour
{

    [SerializeField]
    private int currentHealth, maxHealth;

    private GameObject entity;

    public UnityEvent<GameObject> OnHitWithRefrence;
    public UnityEvent<GameObject, Vector3> OnDeathWithRefrenceAndPosition;
    public UnityEvent OnHeal;

    [SerializeField]
    private bool isDead = false;


    //IFRAMES
    [SerializeField] private float invincibilityTime = 0.1f; // tempo em segundos
    private bool isInvincible = false;

    //You can check if attacker layer is the same as gameObject layer if needed

    public void Awake()
    {
        if (gameObject.tag != "Player")
        {
            entity = transform.parent.gameObject;
        }
    }

    public void InicializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

    public void GetHit(int amount, GameObject attacker)
    {
        if (isDead || isInvincible) return;

        //print("GETHIT");

        currentHealth -= amount;

        if (currentHealth > 0)
        {
            OnHitWithRefrence?.Invoke(attacker);
            TriggerInvincibility(invincibilityTime);
        }
        else
        {
            Vector3 deathPosition = entity != null ? entity.transform.position : Vector3.zero;
            OnDeathWithRefrenceAndPosition?.Invoke(attacker, deathPosition);
            isDead = true;
            if (entity != null)
            {
                Destroy(entity);
            }
        }
    }

    //private IEnumerator StartInvincibility(float delay)
    //{
    //    isInvincible = true;
        
    //    //print("START INVINCIBILITY");

    //    yield return new WaitForSeconds(delay);

    //    isInvincible = false;
    //}

    private void StartInvencibility2()
    {
        isInvincible = false;
    }

    public void TriggerInvincibility(float duration)
    {
        isInvincible = true;
        Action function = StartInvencibility2;

        GlobalCoroutineRunner.Instance.RunCoroutine(function, duration);

        //StartCoroutine(StartInvincibility(duration));
    }


    public void Heal(int amount)
    {
        if (isDead) return;
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        OnHeal?.Invoke();
    }

    public int[] GetHealthAndMaxHealth()
    {
        return new int[] { currentHealth, maxHealth };
    }


    public void SetOnDeathEvent(UnityEvent<GameObject, Vector3> newEvent)
    {
        OnDeathWithRefrenceAndPosition = newEvent;
    }

}
