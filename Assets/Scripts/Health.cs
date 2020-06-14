using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int totalHealth = 20;

    public void DamageHealth(int dmg)
    {
        totalHealth -= dmg;
        GetComponent<Animator>().SetTrigger("takeDamage");
        if (totalHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("die");
    }
}
