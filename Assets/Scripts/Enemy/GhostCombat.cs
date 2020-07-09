using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCombat : MonoBehaviour
{

    [SerializeField] protected float attackRange = 5f;
    [SerializeField] protected float moveToPlayerSpeed = 3f;
    [SerializeField] protected Transform BreathTransform;
    [SerializeField] protected GameObject breathPrefab;
    [SerializeField] protected Quaternion breathRotation;
    [SerializeField] protected float breathRadius = 1.4f;
    [SerializeField] protected float timeBetweenAttacks = 2f;
    [SerializeField] protected float breathDamage = 2f;


    protected Vector2 directionVector;
    protected Health health;
    protected Transform player;
    protected Animator _animator;
    protected EnemyMovement movement;
    public bool isAttacking { get; set; }
    protected float breathOffsetX;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<EnemyHealth>();
        _animator = GetComponent<Animator>();
        movement = GetComponent<EnemyMovement>();
        breathOffsetX = Mathf.Abs(BreathTransform.position.x - transform.position.x);
    }

    void Update()
    {
        FindPlayer();
        if(player != null) InitiateAttack();
    }

    protected void InitiateAttack()
    {
        if (!health.IsDead())
        {
            if (!isAttacking)
            {
                isAttacking = true;
                movement.homingIn = true;
                CheckDirection();
                StartCoroutine(ChasePlayer());
            }
        }
    }

    private void FindPlayer()
    {
        Collider2D[] enemiesFound = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Player"));
        foreach (Collider2D enemy in enemiesFound)
        {
            if (enemy.transform.tag == "Player")
            {
                player = enemy.transform;
            }
        }
    }

    private void AttackPlayer(Collider2D[] enemies)
    {
        //stop movement
        StartCoroutine(movement.Damaged());
        OnAttack();
        //DealDamage(enemies);
    }

    private void DealDamage(Collider2D[] enemies)
    {
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.GetComponent<PlayerHealth>())
                enemy.GetComponent<PlayerHealth>().DamageHealth(breathDamage);
        }
    }

    protected virtual void OnAttack()
    {
        var breath = Instantiate(breathPrefab, BreathTransform.position, breathRotation);
        breath.GetComponent<Breath>().SetDamage(breathDamage);
        breath.GetComponent<Animator>().SetTrigger("Blue");
    }

    private IEnumerator ChasePlayer()
    {
        Collider2D[] enemiesHit;
        do
        {
            directionVector = player.position - BreathTransform.position;
            directionVector = directionVector.normalized;
            movement.SetMovementVector(directionVector * moveToPlayerSpeed);
            enemiesHit = Physics2D.OverlapCircleAll(BreathTransform.position, breathRadius, LayerMask.GetMask("Player"));
            CheckDirection();
            yield return new WaitForSeconds(Time.deltaTime);
        } while (enemiesHit.Length == 0);
        AttackPlayer(enemiesHit);
        yield return new WaitForSeconds(timeBetweenAttacks);
        movement.homingIn = false;
        OnAttackCompleted();
    }

    protected virtual void OnAttackCompleted()
    {
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
    }
}
