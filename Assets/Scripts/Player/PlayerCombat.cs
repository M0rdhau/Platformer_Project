using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combat")]

    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float attackDamage = 5f;
    [SerializeField] float attackRate = 2f;
    [SerializeField] float kickOffset = 0.8f;
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
                var kickPos = attackPoint.position;
                kickPos.y -= kickOffset;
                attackPoint.position = kickPos;
                Attack("kick");
                kickPos.y += kickOffset;
                attackPoint.position = kickPos;
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
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
        {
            _animator.SetBool("kickAerial", true);
        }
        else
        {
            _animator.SetTrigger(attName);
        }
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, LayerMask.GetMask("Enemies"));
        if (enemiesHit.Length > 0)
        {
            if (_animator.GetBool("kickAerial"))
            {
                _animator.SetBool("kickAerial", false);
                _animator.SetTrigger("enemyHitAerial");
                if (GetComponent<SpriteRenderer>().flipX)
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.right;
                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.left;
                }
            }
        }
        foreach (Collider2D enemy in enemiesHit)
        {
            if (enemy.GetComponent<Health>() != null && !enemy.GetComponent<Health>().IsDead())
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
