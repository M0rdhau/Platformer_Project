using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] float explosionRadius = 3f;
    [SerializeField] float damage = 3f;
    [SerializeField] string[] layerMask;


    private void Start()
    {
        if (GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Hit(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask(layerMask)))
        {
            Hit(collision.gameObject);
        }
    }

    private void Hit(GameObject collision)
    {
        GetComponent<ParticleSystem>().Stop();
        Explode();
        CollideWithHittable(collision);
    }

    public void SetDamage(float dmgSet)
    {
        damage = dmgSet;
    }


    private void CollideWithHittable(GameObject collision)
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, explosionRadius, LayerMask.GetMask(layerMask));
        //Should be only one player
        if (enemiesHit.Length > 0)
        {
            foreach (Collider2D enemy in enemiesHit)
            {
                bool knockedRight = IsEnemyRight(enemy.transform);
                enemy.GetComponent<Health>().KnockBackHit(damage, knockedRight);
            }   
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

    public void ReverseDirection()
    {
        GetComponent<Rigidbody2D>().velocity = -GetComponent<Rigidbody2D>().velocity;
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
