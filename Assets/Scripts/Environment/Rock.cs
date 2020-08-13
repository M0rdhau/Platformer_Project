using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] float damageVelocity = 6f;
    [SerializeField] GameObject destructionPrefab;
    [SerializeField] AudioClip damagedClip;

    Rigidbody2D rBody;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && rBody.velocity.magnitude > 0)
        {
            collision.gameObject.GetComponent<PlayerHealth>().DamageHealth(rBody.velocity.magnitude / damageVelocity);
            DestroyBoulder();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyProjectile")
        {
            DestroyBoulder();
        }
    }

    private void DestroyBoulder()
    {
        AudioSource.PlayClipAtPoint(damagedClip, transform.position, 1f);
        GameObject destParticles = Instantiate(destructionPrefab, transform.position, transform.rotation);
        gameObject.SetActive(false);
        Destroy(destParticles, 3);
        Destroy(gameObject, 4);
    }

}
