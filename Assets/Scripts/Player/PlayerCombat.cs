using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combat")]

    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float aerialRange = 0.55f;
    [SerializeField] float attackDamage = 5f;
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
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                Attack("kick");
                nextAttackTime = Time.time + 1f / attackRate;
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
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
        float range;
        Collider2D[] enemiesHit;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
        {
            _animator.SetBool("kickAerial", true);
            range = aerialRange;
        }
        else
        {
            _animator.SetTrigger(attName);
            range = attackRange;
        }
        enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, range, LayerMask.GetMask("Enemies"));
        foreach (Collider2D enemy in enemiesHit)
        {
            if (enemy.GetComponent<Health>() != null && !enemy.GetComponent<Health>().IsDead())
            {
                if (_animator.GetBool("kickAerial"))
                {
                    enemy.GetComponent<Health>().KnockBackHit(attackDamage);
                } else
                {
                    enemy.GetComponent<Health>().DamageHealth(attackDamage);
                }
            }
        }
        if (enemiesHit.Length > 0)
        {
            if (_animator.GetBool("kickAerial"))
            {
                _animator.SetBool("kickAerial", false);
                _animator.SetTrigger("enemyHitAerial");
                if (GetComponentInChildren<SpriteRenderer>().flipX)
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.right;
                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.left;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
