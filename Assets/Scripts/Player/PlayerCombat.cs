using System;
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
    [SerializeField] Transform airAttackPoint;
    float nextAttackTime = 0f;
    float actualRange;
    LayerMask enemyLayers;
    Animator _animator;
    SpriteRenderer _renderer;
    CombatCharge charge;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        charge = GetComponent<CombatCharge>();
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
        Vector2 airPos = airAttackPoint.position;
        pos.x += 2 * diff;
        airPos.x += 2 * diff;
        attackPoint.position = pos;
        airAttackPoint.position = airPos;
    }

    private void Attack(string attName)
    {
        CheckAttackPoint();
        PlayAnim(attName);
        StartCoroutine(HitAndDamage());
    }

    private IEnumerator HitAndDamage()
    {
        Transform attackTransform;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("AirKick"))
        {
            attackTransform = airAttackPoint;
        }
        else
        {
            attackTransform = attackPoint;
        }
        Collider2D[] enemies;
        do
        {
            enemies = Physics2D.OverlapCircleAll(attackTransform.position, actualRange, LayerMask.GetMask("Enemies"));
            yield return null;
        } while (enemies.Length == 0 && _animator.GetCurrentAnimatorStateInfo(0).IsName("AirKick"));
        DamageEnemies(enemies);
    }


    private void DamageEnemies(Collider2D[] enemiesHit)
    {
        foreach (Collider2D enemy in enemiesHit)
        {
            charge.AddCharge(attackDamage*(1 + charge.GetCharge()));
            bool knockedRight = IsEnemyRight(enemy);
            if (enemy.tag == "EnemyProjectile")
            {
                enemy.GetComponent<Fireball>().Explode();
                return;
            }

            if (enemy.GetComponent<Health>() != null && !enemy.GetComponent<Health>().IsDead())
            {
                enemy.GetComponent<Health>().KnockBackHit(attackDamage * (1 + charge.GetCharge()), knockedRight);
            }
        }
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

    private bool IsEnemyRight(Collider2D enemy)
    {
        bool knockedRight;
        if (enemy.transform.position.x > transform.position.x)
        {
            knockedRight = true;
        }
        else
        {
            knockedRight = false;
        }

        return knockedRight;
    }

    private void PlayAnim(string attName)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
        {
            _animator.SetBool("kickAerial", true);
            actualRange = aerialRange;
        }
        else
        {
            _animator.SetTrigger(attName);
            actualRange = attackRange;
        }
    }

    

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(airAttackPoint.position, aerialRange);

    }

}
