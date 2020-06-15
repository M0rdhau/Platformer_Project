using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combat")]

    [SerializeField] float attackRange = 0.8f;
    [SerializeField] int attackDamage = 5;
    [SerializeField] float attackRate = 2f;
    [SerializeField] Transform attackPoint;
    float nextAttackTime = 0f;
    LayerMask enemyLayers;
    Animator _animator;
    SpriteRenderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        enemyLayers = LayerMask.GetMask("Enemies");

    }

    // Update is called once per frame
    void Update()
    {
        HandleAttackInput();
    }


    private void HandleAttackInput()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetAxis("Fire1") > 0)
            {
                Attack("kick");
                nextAttackTime = Time.time + 1f / attackRate;
            }
            if (Input.GetAxis("Fire2") > 0)
            {
                Attack("punch");
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    private void CheckAttackPoint()
    {
        var diff = transform.position.x - attackPoint.position.x;
        if ((_renderer.flipX && diff < 0) || (!_renderer.flipX && diff > 0))
        {
            FlipAttackPoint(diff);
        }
    }

    void FlipAttackPoint(float diff)
    {
        Vector2 pos = attackPoint.position;
        pos.x += 2 * diff;
        attackPoint.position = pos;
    }

    private void Attack(string attName)
    {
        CheckAttackPoint();
        _animator.SetTrigger(attName);
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D enemy in enemiesHit)
        {
            if (enemy.GetComponent<Health>())
            {
                enemy.GetComponent<Health>().DamageHealth(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
