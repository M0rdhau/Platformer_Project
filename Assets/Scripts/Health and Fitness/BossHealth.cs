using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour, Health
{
    [SerializeField] float maxHealth = 20f;
    [SerializeField] float totalHealth = 20f;
    [SerializeField] ParticleSystem deathSystem;
    [SerializeField] float waitforWinScreen = 5f;
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
            //StartCoroutine(GetComponent<EnemyMovement>().Damaged());
            DecreaseHealth(dmg);
        }
    }

    private void DecreaseHealth(float dmg)
    {
        totalHealth -= dmg;
        GetComponent<BossCombat>().isAttacking = false;
        anim.SetTrigger("takeDamage");
        if (totalHealth <= 0)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        isDead = true;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponent<BossMovement>().enabled = false;
        var fireballs = GetComponent<BossCombat>().GetFireBalls();
        foreach (GameObject fireball in fireballs)
        {
            fireball.SetActive(false);
        }
        GetComponent<Animator>().speed = 0;
        var enemySpawners = FindObjectsOfType<EnemySpawner>();
        foreach (EnemySpawner spawner in enemySpawners)
        {
            spawner.gameObject.SetActive(false);
        }
        deathSystem.Play();
        this.enabled = false;
        StartCoroutine(Win());
    }

    IEnumerator Win()
    {
        yield return new WaitForSeconds(waitforWinScreen);
        Debug.Log("You won!");
    }

    public float GetHealth()
    {
        return totalHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void Die()
    {
        Destroy(gameObject, 3f);
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void KnockBackHit(float dmg, bool knockedRight)
    {
        DamageHealth(dmg);
    }

}
