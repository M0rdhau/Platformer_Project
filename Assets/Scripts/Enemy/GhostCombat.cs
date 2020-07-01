using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCombat : MonoBehaviour
{

    [SerializeField] float attackRange = 5f;
    [SerializeField] float moveToPlayerSpeed = 3f;
    [SerializeField] float distanceFromPlayer = 2f;
    [SerializeField] Transform BreathTransform;
    [SerializeField] GameObject breathPrefab;
    [SerializeField] Quaternion breathRotation;
    [SerializeField] float breathRadius = 1.4f;
    [SerializeField] float timeBetweenAttacks = 2f;
    [SerializeField] float breathDamage = 2f;
    

    Vector2 directionVector;
    EnemyHealth health;
    Transform player;
    Animator _animator;
    EnemyMovement movement;
    bool isAttacking;
    float breathOffsetX;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<EnemyHealth>();
        _animator = GetComponent<Animator>();
        movement = GetComponent<EnemyMovement>();
        breathOffsetX = Mathf.Abs(BreathTransform.position.x - transform.position.x);
    }

    // Update is called once per frame
    void Update()
    {
        if (!health.IsDead())
        {
            Collider2D[] enemiesFound = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Player"));
            if (enemiesFound.Length > 0 && !isAttacking)
            {
                isAttacking = true;
                movement.homingIn = true;
                player = enemiesFound[0].transform;
                CheckDirection();
                StartCoroutine(ChasePlayer());
            }
        }
    }

    private void AttackPlayer(Collider2D[] enemies)
    {
        var breath = Instantiate(breathPrefab, BreathTransform.position, breathRotation);
        Destroy(breath, 0.5f);
        foreach (Collider2D enemy in enemies)
        {
            enemy.GetComponent<PlayerHealth>().DamageHealth(breathDamage);
        }
    }

    private IEnumerator ChasePlayer()
    {
        Collider2D[] enemiesHit;
        do
        {
            directionVector = player.position - BreathTransform.position;
            directionVector = directionVector.normalized;
            movement.SetMovementVector(directionVector*moveToPlayerSpeed);
            enemiesHit = Physics2D.OverlapCircleAll(BreathTransform.position, breathRadius, LayerMask.GetMask("Player"));
            CheckDirection();
            yield return new WaitForSeconds(Time.deltaTime);
        } while (enemiesHit.Length == 0);
        AttackPlayer(enemiesHit);
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    private void CheckDirection()
    {
        var _renderer = GetComponentInChildren<SpriteRenderer>();
        if (player.transform.position.x > transform.position.x)
        {
            _renderer.flipX = true;
            var vec = BreathTransform.position;
            vec.x = transform.position.x + breathOffsetX;
            BreathTransform.position = vec;
            breathRotation = Quaternion.Euler(0, 180, 0);

        }
        else if (player.transform.position.x < transform.position.x)
        {
            _renderer.flipX = false;
            var vec = BreathTransform.position;
            vec.x = transform.position.x - breathOffsetX;
            BreathTransform.position = vec;
            breathRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(BreathTransform.position, breathRadius);
        var gizmoColor = Color.red;
        gizmoColor.a = 0.2f;
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, distanceFromPlayer);
    }
}
