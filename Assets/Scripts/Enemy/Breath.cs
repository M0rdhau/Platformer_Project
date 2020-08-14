using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breath : MonoBehaviour
{
    private float damage = 0f;
    [SerializeField] Vector2 dimensions = new Vector2(5f, 2.5f);
    [SerializeField] AudioClip bossSound;
    [SerializeField] AudioClip ghostSound;


    //set damage, color of fire
    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void PlayBossSound()
    {
        AudioSource.PlayClipAtPoint(bossSound, transform.position, 1f);
    }

    public void PlayGhostSound()
    {
        AudioSource.PlayClipAtPoint(ghostSound, transform.position, 1f);
    }

    private void DamageEnemies()
    {
        var enemies = Physics2D.OverlapBoxAll(transform.position, dimensions, 0f, LayerMask.GetMask("Player"));
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.tag == "Player")
            {
                enemy.GetComponent<Health>().DamageHealth(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, dimensions);

    }

}
