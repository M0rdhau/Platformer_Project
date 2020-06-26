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
    [SerializeField] float projLifeTime = 6f;


    Vector2 directionVector;
    Transform player;
    Animator _animator;

    float timeSinceShot = 0f;
    float fireballOffsetX;

    private void Start()
    {
        _animator = GetComponent<Animator>();
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
            CheckDirection();
            _animator.SetTrigger("CastFireball");
        }
    }

    public void Shoot()
    {
        directionVector = player.position - fireballTransform.position;
        directionVector = directionVector.normalized;
        var fireball = Instantiate(fireballPrefab, fireballTransform.position, transform.rotation);
        fireball.GetComponent<Fireball>().SetMoveVector(directionVector * projSpeed);
        StartCoroutine(TrackProjectile(fireball));
    }

    IEnumerator TrackProjectile(GameObject fBall)
    {
        yield return new WaitForSeconds(projLifeTime);
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
            var vec = fireballTransform.position;
            vec.x += fireballOffsetX*2;
            fireballTransform.position = vec;
        }
        else if ((player.transform.position.x < transform.position.x) && _renderer.flipX)
        {
            _renderer.flipX = false;
            var vec = fireballTransform.position;
            vec.x -= fireballOffsetX*2;
            fireballTransform.position = vec;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
