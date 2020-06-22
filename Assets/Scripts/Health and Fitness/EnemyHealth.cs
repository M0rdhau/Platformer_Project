using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, ISaveable, Health
{
    [SerializeField] float totalHealth = 20f;
    Animator anim;
    bool isDead = false;


    private void Start()
    {
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
            StartCoroutine(GetComponent<EnemyMovement>().KnockBack(knockedRight));
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
        GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }



    public object CaptureState()
    {
        return totalHealth;
    }

    public void RestoreState(object state)
    {
        totalHealth = (float)state;
        if (totalHealth <= 0)
        {
            isDead = true;
            Die();
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
}
