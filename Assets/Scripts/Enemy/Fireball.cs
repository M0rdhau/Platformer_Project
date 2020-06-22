using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] float explosionRadius = 3f;
    [SerializeField] float damage = 3f;

    private void Start()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, explosionRadius, LayerMask.GetMask("Player"));
        //Should be only one player
        if (enemiesHit.Length > 0)
        {
            bool knockedRight = IsEnemyRight(collision.transform);

            enemiesHit[0].GetComponent<Health>().KnockBackHit(damage, true);
        }
    }

    private bool IsEnemyRight(Transform enemy)
    {
        bool knockedRight;
        if (enemy.position.x > transform.position.x)
        {
            knockedRight = true;
        }
        else
        {
            knockedRight = false;
        }

        return knockedRight;
    }

    public void Explode()
    {
        GetComponent<Animator>().SetTrigger("explode");
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    public void SetMoveVector(Vector2 vec)
    {
        GetComponent<Rigidbody2D>().velocity = vec;
    }

    public void Die()
    {
        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
