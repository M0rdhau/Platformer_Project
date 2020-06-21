using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardCombat : MonoBehaviour
{
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] float shootFrequency = 0.5f;
    [SerializeField] float attackRange = 5f;
    [SerializeField] float projSpeed = 4f;
    [SerializeField] Transform fireballTransform;
    [SerializeField] float fBallLifeTime = 6f;


    Vector2 directionVector;
    Transform player;

    float timeSinceShot = 0f;
    float fireballOffsetX;

    private void Start()
    {
        fireballOffsetX = Mathf.Abs(fireballTransform.position.x - transform.position.x);
    }


    // Update is called once per frame
    void Update()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Player"));

        if (Time.time >= timeSinceShot && enemiesHit.Length > 0)
        {
            player = enemiesHit[0].transform;
            timeSinceShot = Time.time + 1f / shootFrequency;
            Shoot();
        }
    }

    void Shoot()
    {
        directionVector = player.position - transform.position;
        directionVector = directionVector.normalized;
        CheckDirection();
        var fireball = Instantiate(fireballPrefab, transform.position, transform.rotation);
        fireball.GetComponent<Fireball>().SetMoveVector(directionVector * projSpeed);
        StartCoroutine(TrackFireball(fireball));
    }

    IEnumerator TrackFireball(GameObject fBall)
    {
        yield return new WaitForSeconds(fBallLifeTime);
        if (fBall)
        {
            fBall.GetComponent<Animator>().SetTrigger("explode");
        }
    }

    private void CheckDirection()
    {
        var _renderer = GetComponentInChildren<SpriteRenderer>();
        if ((player.transform.position.x > transform.position.x) && !_renderer.flipX)
        {
            _renderer.flipX = true;
            var vec = transform.position;
            vec.x += fireballOffsetX;
            fireballTransform.position = vec;
        }
        else if ((player.transform.position.x < transform.position.x) && _renderer.flipX)
        {
            _renderer.flipX = false;
            var vec = transform.position;
            vec.x -= fireballOffsetX;
            fireballTransform.position = vec;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
