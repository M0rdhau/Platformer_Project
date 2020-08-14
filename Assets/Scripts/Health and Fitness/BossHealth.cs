using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour, Health
{
    [SerializeField] float maxHealth = 20f;
    [SerializeField] float totalHealth = 20f;
    [SerializeField] ParticleSystem deathSystem;
    [SerializeField] float waitforWinScreen = 5f;
    [SerializeField] float secondsDamaged = 1f;

    [SerializeField] AudioClip damagedClip;
    [SerializeField] AudioClip deathClip;

    Animator anim;
    SpriteRenderer _renderer;
    bool isDead = false;

    private void Start()
    {
        totalHealth = maxHealth;
        anim = GetComponent<Animator>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
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
        if (damagedClip != null)
        {
            AudioSource.PlayClipAtPoint(damagedClip, transform.position, 1f);
        }
        totalHealth -= dmg;
        StartCoroutine(Damaged());
        if (totalHealth <= 0)
        {
            HandleDeath();
        }
    }

    IEnumerator Damaged()
    {
        _renderer.color = Color.red;
        yield return new WaitForSeconds(secondsDamaged);
        _renderer.color = Color.white;
    }

    private void HandleDeath()
    {
        if (deathClip != null)
        {
            AudioSource.PlayClipAtPoint(deathClip, transform.position, 1f);
        }

        isDead = true;
        _renderer.enabled = false;
        GetComponent<BossMovement>().enabled = false;
        var fireballs = GetComponent<BossCombat>().GetFireBalls();
        foreach (GameObject fireball in fireballs)
        {
            if (fireball != null) ;
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
        Alert al = FindObjectOfType<Alert>();
        al.ReceiveText("You won! But there's still no escape. Have fun!");

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
