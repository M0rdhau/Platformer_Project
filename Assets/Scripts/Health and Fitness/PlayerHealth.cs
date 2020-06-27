using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System;

public class PlayerHealth : MonoBehaviour, ISaveable, Health
{
    [SerializeField] float totalHealth = 20f;
    [SerializeField] float maxHealth = 20f;
    [SerializeField] float invulnerableTime = 1f;
    [SerializeField] GameObject invulLight;
    PlayerUIHandler handler;
    Animator anim;
    bool isDead = false;
    bool isInvulnerable = false;


    private void Start()
    {
        handler = GetComponent<PlayerUIHandler>();
        anim = GetComponent<Animator>();
        handler.UpdateHealth(totalHealth);
    }

    public void KnockBackHit(float dmg, bool knockedRight)
    {
        if (!isInvulnerable)
        {
            DamageHealth(dmg);
            GetComponent<PlayerController>().KnockBack(knockedRight);
        }
    }

    public void DamageHealth(float dmg)
    {
        
        if (!isInvulnerable)
        {
            if (!isDead)
            {
                GetComponent<PlayerCombat>().DisruptFirePunch(false);
                StartCoroutine(BecomeInvulnerable());
                totalHealth -= dmg;
                handler.UpdateHealth(totalHealth);            
                if (totalHealth <= 0)
                {
                    HandleDeath();
                }
            }
        }
    }

    private IEnumerator BecomeInvulnerable()
    {
        invulLight.GetComponent<Light2D>().intensity = 20f;
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerableTime);
        invulLight.GetComponent<Light2D>().intensity = 0;
        isInvulnerable = false;
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

    
}
