using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, Health
{
    [SerializeField] float maxHealth = 20f;
    [SerializeField] float totalHealth = 20f;
    Animator anim;
    bool isDead = false;


    private void Start()
    {
        totalHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    public void DamageHealth(float dmg)
    {
        if (!isDead)
        {
            StartCoroutine(GetComponent<EnemyMovement>().Damaged());
            DecreaseHealth(dmg);
        }
    }

    public void KnockBackHit(float dmg, bool knockedRight)
    {
        if (!isDead)
        {
            if(GetComponent<EnemyMovement>()) StartCoroutine(GetComponent<EnemyMovement>().KnockBack(knockedRight));
            DecreaseHealth(dmg);
        }
    }

    private void DecreaseHealth(float dmg)
    {
        totalHealth -= dmg;
        anim.SetTrigger("takeDamage");
        if (totalHealth <= 0)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        anim.SetBool("isDead", true);
        isDead = true;
        this.enabled = false;
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }

    public bool IsDead()
    {
        return isDead;
    }

    public float GetHealth()
    {
        return totalHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
