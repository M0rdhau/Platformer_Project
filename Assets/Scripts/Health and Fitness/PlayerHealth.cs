using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, ISaveable, Health
{
    [SerializeField] float totalHealth = 20f;
    [SerializeField] float maxHealth = 20f;
    [SerializeField] float invulnerableTime = 0.2f;
    PlayerUIHandler handler;
    float timeSinceHit = 0;
    Animator anim;
    bool isDead = false;


    private void Start()
    {
        handler = GetComponent<PlayerUIHandler>();
        anim = GetComponent<Animator>();
        handler.UpdateHealth(totalHealth);
    }

    public void DamageHealth(float dmg)
    {
        if (Time.time - timeSinceHit > invulnerableTime)
        {
            GetComponent<Animator>().SetTrigger("Invulnerable");
            timeSinceHit = Time.time;
            totalHealth -= dmg;
            handler.UpdateHealth(totalHealth);
            if (!isDead)
            {
                //anim.SetTrigger("takeDamage");
                if (totalHealth <= 0)
                {
                    HandleDeath();
                }
            }
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
        GetComponent<Collider2D>().enabled = false;
    }

    public void Heal(float healing)
    {
        totalHealth = Mathf.Clamp(totalHealth + healing, 0, maxHealth);
        handler.UpdateHealth(totalHealth);
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

    public void KnockBackHit(float dmg, bool knockedRight)
    {
        DamageHealth(dmg);
        GetComponent<PlayerController>().KnockBack(knockedRight);
    }
}
