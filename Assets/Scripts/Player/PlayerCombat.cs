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
    [SerializeField] float chargeRate = 30f;
    [SerializeField] float projSpeed = 6f;
    [SerializeField] float projLifeTime = 6f;
    [SerializeField] Transform attackPoint;
    [SerializeField] Transform airAttackPoint;
    [SerializeField] Transform punchTransform;
    [SerializeField] GameObject punchProjectile;
    float nextAttackTime = 0f;
    float punchWaitTime = 2f;
    float chargeBeforePunch = 0f;
    float actualRange;

    LayerMask enemyLayers;
    Animator _animator;
    SpriteRenderer _renderer;
    CombatCharge charge;
    IEnumerator punchCoroutine;
    PlayerUpgrades upgrades;

    bool chargingFist = false;

    // Start is called before the first frame update
    void Start()
    {
        upgrades = GetComponent<PlayerUpgrades>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        charge = GetComponent<CombatCharge>();
        enemyLayers = LayerMask.GetMask("Enemies");
        //punchCoroutine = WaitForPunchTime();
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
             if (Input.GetKeyDown(KeyCode.W))
            {
                Attack("kick");
                nextAttackTime = Time.time + 1f / attackRate;
            }


            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && upgrades.HasUpgrade(Upgrade.UpgradeType.FireFist))
                {
                    nextAttackTime = Time.time + 1f / attackRate;
                    chargeBeforePunch = charge.GetCharge();
                    punchCoroutine = WaitForPunchTime();
                    StartFirePunch();
                }
                else
                {
                    Attack("punch");
                }
                
            }
        }
        if (Input.GetKeyUp(KeyCode.E) && chargingFist)
        {
            Debug.Log("Stopping the punch charge");
            DisruptFirePunch();
        }

        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.E) && chargingFist)
        {
            //just stops the charging if any other key is pressed without dealing damage
            Debug.Log("Stopping the punch charge");
            DisruptFirePunch(false);
        }
    }

    private void StartFirePunch()
    {
        chargingFist = true;
        CheckAttackPoint();
        DelayPunch();
        PlayAnim("punch");
    }

    private void DelayPunch()
    {
        _animator.speed = 0;
        punchTransform.gameObject.GetComponent<ParticleSystem>().Play();
        StartCoroutine(punchCoroutine);
    }

    private void DisruptFirePunch(bool shouldHit = true)
    {
        StopCoroutine(punchCoroutine);
        punchCoroutine = WaitForPunchTime();
        FinalizePunch(shouldHit);
    }

    IEnumerator WaitForPunchTime()
    {
        while (Time.time - nextAttackTime < (1 - charge.GetCharge())*punchWaitTime)
        {
            charge.AddCharge(charge.GetMaxDamage()/chargeRate, true);
            yield return new WaitForSeconds((1 - charge.GetCharge())*punchWaitTime/chargeRate);
        }
        FinalizePunch(true);
        ShootPojectile();
    }

    private void ShootPojectile()
    {
        var fireFist = Instantiate(punchProjectile, attackPoint.position, transform.rotation);
        Vector2 directionVector;
        if (attackPoint.position.x > transform.position.x)
        {
            directionVector = Vector2.right;
        }
        else
        {
            directionVector = Vector2.left;
        }
        fireFist.GetComponent<Fireball>().SetMoveVector(directionVector * projSpeed);
        fireFist.GetComponent<Fireball>().SetDamage(attackDamage * (1 + charge.GetCharge()));
        StartCoroutine(TrackProjectile(fireFist));
    }

    IEnumerator TrackProjectile(GameObject fBall)
    {
        yield return new WaitForSeconds(projLifeTime);
        if (fBall)
        {
            fBall.GetComponent<Animator>().SetTrigger("explode");
        }
    }

    private void FinalizePunch(bool shouldHit)
    {
        punchTransform.gameObject.GetComponent<ParticleSystem>().Stop();
        _animator.speed = 1;
        if(shouldHit) StartCoroutine(HitAndDamage());
    }

    private void CheckAttackPoint()
    {
        var diff = transform.position.x - attackPoint.position.x;
        var fistDiff = transform.position.x - punchTransform.position.x;
        if ((_renderer.flipX && diff < 0) || (!_renderer.flipX && diff > 0))
        {
            FlipAttackPoint(diff);
            FlipPunchPoint(fistDiff);
        }
    }

    private void FlipPunchPoint(float fistDiff)
    {
        Vector2 pos = punchTransform.position;
        pos.x += 2 * fistDiff;
        punchTransform.position = pos;
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
            enemies = Physics2D.OverlapCircleAll(attackTransform.position, actualRange, LayerMask.GetMask("Enemies", "Projectiles"));
            yield return null;
        } while (enemies.Length == 0 && _animator.GetCurrentAnimatorStateInfo(0).IsName("AirKick"));
        DamageEnemies(enemies);
    }


    private void DamageEnemies(Collider2D[] enemiesHit)
    {
        float damage = attackDamage * (1 + charge.GetCharge());
        ResetChargeFromAttack();
        foreach (Collider2D enemy in enemiesHit)
        {
            charge.AddCharge(damage);
            bool knockedRight = IsEnemyRight(enemy);
            if (enemy.tag == "EnemyProjectile")
            {
                enemy.GetComponent<Fireball>().ReverseDirection();
                return;
            }

            if (enemy.GetComponent<Health>() != null && !enemy.GetComponent<Health>().IsDead())
            {
                enemy.GetComponent<Health>().KnockBackHit(damage, knockedRight);
            }
        }
        if (_animator.GetBool("kickAerial"))
        {
            HandleAirKickAnim();
        }
    }

    private void HandleAirKickAnim()
    {
        _animator.SetBool("kickAerial", false);
        _animator.SetTrigger("enemyHitAerial");
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        var rigidBody = GetComponent<Rigidbody2D>();
        if (spriteRenderer.flipX)
        {
            rigidBody.velocity = 2*Vector2.right;
        }
        else
        {
            rigidBody.velocity = 2*Vector2.left;
        }
    }

    private void ResetChargeFromAttack()
    {
        if (chargingFist)
        {
            chargingFist = false;
            charge.ResetCharge(chargeBeforePunch);
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
