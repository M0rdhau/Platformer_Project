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
    [SerializeField] float deathWaitTime = 1f;
    [SerializeField] GameObject invulLight;
    [SerializeField] GameObject deathCanvas;
    PlayerUIHandler handler;
    PlayerController controller;
    Animator anim;
    bool isDead = false;
    bool isInvulnerable = false;


    private void Start()
    {
        handler = FindObjectOfType<PlayerUIHandler>();
        anim = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        handler.UpdateHealth(totalHealth);
    }

    public void KnockBackHit(float dmg, bool knockedRight)
    {
        if (!isInvulnerable && !controller.GetRolling())
        {
            DamageHealth(dmg);
            controller.KnockBack(knockedRight);
        }
    }

    public void DamageHealth(float dmg)
    {
        
        if (!isInvulnerable && !controller.GetRolling())
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
        //this boolean doesn't matter much.
        controller.KnockBack(true);
        isDead = true;
        //this.enabled = false;
    }

    public void Die()
    {
        if (isDead)
        {
            anim.SetBool("isDead", true);
            StartCoroutine(DeathCoroutine());
        }
    }

    private IEnumerator DeathCoroutine()
    {

        Time.timeScale = 0;
        deathCanvas.SetActive(true);
        yield return new WaitForSecondsRealtime(deathWaitTime);
        deathCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        
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

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealth()
    {
        return totalHealth;
    }

}
