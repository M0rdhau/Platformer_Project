using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, ISaveable, Health
{
    [SerializeField] float totalHealth = 20f;
    [SerializeField] float maxHealth = 20f;
    [SerializeField] float invulnerableTime = 1f;
    [SerializeField] GameObject light;
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
            if (!isDead)
            {
                StartCoroutine(ShedLight());
                timeSinceHit = Time.time;
                totalHealth -= dmg;
                handler.UpdateHealth(totalHealth);            
                if (totalHealth <= 0)
                {
                    HandleDeath();
                }
            }
        }
    }

    private IEnumerator ShedLight()
    {
        light.GetComponent<Light>().intensity = 20f;
        yield return new WaitForSeconds(1f);
        light.GetComponent<Light>().intensity = 0;
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
        if (Time.time - timeSinceHit > invulnerableTime)
        {
            DamageHealth(dmg);
            GetComponent<PlayerController>().KnockBack(knockedRight);
        }
    }
}
