using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] float damage = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DealDamage(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        DealDamage(collision);
    }

    private void DealDamage(Collider2D collision)
    {
        if (collision.GetComponent<PlayerHealth>())
        {
            bool knockedRight = IsEnemyRight(collision);
            collision.GetComponent<PlayerHealth>().KnockBackHit(damage, true);
        }
    }

    private bool IsEnemyRight(Collider2D enemy)
    {
        bool knockedRight;
        if (enemy.transform.position.x > transform.position.x)
        {
            knockedRight = true;
        }
        else
        {
            knockedRight = false;
        }

        return knockedRight;
    }
}
