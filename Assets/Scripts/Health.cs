using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int totalHealth = 20;
    Animator anim;
    bool isDead = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void DamageHealth(int dmg)
    {
        if (!isDead)
        {
            totalHealth -= dmg;
            anim.SetTrigger("takeDamage");
            if (totalHealth <= 0)
            {
                HandleDeath();
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
        Destroy(gameObject);
    }

}
