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
    [SerializeField] Vector2 kickAttackBoxParams = new Vector2(3f, 3f);
    [SerializeField] Vector2 punchAttackBoxParams = new Vector2(3f, 3f);
    [SerializeField] Vector2 crouchKickAttackBoxParams = new Vector2(3f, 3f);
    [SerializeField] Transform attackPoint;
    [SerializeField] Transform kickTransform;
    [SerializeField] Transform punchTransform;
    [SerializeField] Transform crouchKickTransform;
    [SerializeField] GameObject punchProjectile;
    [SerializeField] string[] damageLayers = { "Enemies", "Projectiles"};
    float nextAttackTime = 0f;
    float punchWaitTime = 2f;
    float chargeBeforePunch = 0f;
    float actualRange;

    Animator _animator;
    SpriteRenderer _renderer;
    CombatCharge charge;
    IEnumerator punchCoroutine;
    PlayerUpgrades upgrades;
    Transform actualAttackTransform;
    Vector2 actualAttackParams;

    bool chargingFist = false;

    // Start is called before the first frame update
    void Start()
    {
        upgrades = GetComponent<PlayerUpgrades>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        charge = GetComponent<CombatCharge>();
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
                if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && upgrades.HasUpgrade(Upgrade.UpgradeType.FireFist))
                {
                    nextAttackTime = Time.time + 1f / attackRate;
                    chargeBeforePunch = charge.GetCharge();
                    punchCoroutine = WaitForPunchTime();
                    StartFirePunch();
                }
                else
                {
                    int a = UnityEngine.Random.Range(0, 2);
                    if (a == 0)
                    {
                        Attack("punch");
                    }
                    else
                    {
                        Attack("kick");
                    }
                    nextAttackTime = Time.time + 1f / attackRate;
                }

            }
        }
        if (Input.GetKeyUp(KeyCode.W) && chargingFist)
        {
            DisruptFirePunch();
        }

        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.W) && chargingFist)
        {
            //just stops the charging if any other key is pressed without dealing damage
            DisruptFirePunch(false);
        }
    }


    #region Fire Punch handling
    private void StartFirePunch()
    {
        GetCorrectTransform("");
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

    public void DisruptFirePunch(bool shouldHit = true)
    {
        GetComponent<AudioSource>().Stop();
        if (chargingFist)
        {
            StopCoroutine(punchCoroutine);
            punchCoroutine = WaitForPunchTime();
            FinalizePunch(shouldHit);
            chargingFist = false;
        }
    }

    IEnumerator WaitForPunchTime()
    {
        GetComponent<AudioSource>().Play();
        while (charge.GetCharge() < 1)
        {
            charge.AddCharge(charge.GetMaxDamage() / chargeRate, true);
            yield return new WaitForSeconds((1 - charge.GetCharge()) * (punchWaitTime / chargeRate));
        }
        GetComponent<AudioSource>().Stop();
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
        if (shouldHit) { HitAndDamage(); }
    }

    private void ResetChargeFromAttack()
    {
        if (chargingFist)
        {
            chargingFist = false;
            charge.ResetCharge(chargeBeforePunch);
        }
    }
    #endregion



    private void Attack(string attName)
    {
        GetCorrectTransform(attName);
        CheckAttackPoint();
        PlayAnim(attName);
        HitAndDamage();
    }

    private void CheckAttackPoint()
    {
        var diff = Mathf.Abs(transform.position.x - actualAttackTransform.position.x);
        var fistDiff = Mathf.Abs(transform.position.x - punchTransform.position.x);
        FlipAttackPoint(diff, fistDiff, _renderer.flipX);
        //FlipPunchPoint(fistDiff, _renderer.flipX);
    }

    private void FlipPunchPoint(float fistDiff, bool isFlipped)
    {
        Vector2 pos = punchTransform.position;
        pos.x += 2 * fistDiff;
        punchTransform.position = pos;
    }

    void FlipAttackPoint(float diff, float chargeDiff, bool isFlipped)
    {
        Vector2 attackPos = actualAttackTransform.position;
        Vector2 chargePos = punchTransform.position;
        if (!isFlipped)
        {
            attackPos.x = transform.position.x + diff;
            chargePos.x = transform.position.x - chargeDiff;
        }
        else
        {
            attackPos.x = transform.position.x - diff;
            chargePos.x = transform.position.x + chargeDiff;
        }
        actualAttackTransform.position = attackPos;
        punchTransform.position = chargePos;
    }

    private void HitAndDamage()
    {
        ResetChargeFromAttack();
        Collider2D[] enemies;
        enemies = Physics2D.OverlapBoxAll(actualAttackTransform.position, actualAttackParams, 0f, LayerMask.GetMask("Enemies", "Projectiles"));
        if (enemies.Length > 0) { DamageEnemies(enemies); }
    }

    private void GetCorrectTransform(string attName)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
        {
            actualAttackTransform = kickTransform;
            actualAttackParams = kickAttackBoxParams;
        } else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Crouch"))
        {
            actualAttackTransform = crouchKickTransform;
            actualAttackParams = crouchKickAttackBoxParams;
        }
        else if (attName == "kick")
        {
            actualAttackTransform = kickTransform;
            actualAttackParams = kickAttackBoxParams;
        }
        else
        {
            actualAttackTransform = attackPoint;
            actualAttackParams = punchAttackBoxParams;
        }
    }

    private void DamageEnemies(Collider2D[] enemiesHit)
    {
        float damage = attackDamage * (1 + charge.GetCharge());
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

    public void ResetAerialKick()
    {
        _animator.SetBool("kickAerial", false);
    }

    private void PlayAnim(string attName)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
        {
            _animator.SetBool("kickAerial", true);
            actualRange = aerialRange;
        } else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Crouch"))
        {
            _animator.SetTrigger("kick");
        }
        else
        {
            _animator.SetTrigger(attName);
            actualRange = attackRange;
        }
    }

    

    private void OnDrawGizmosSelected()
    {
        //Gizmos.DrawWireCube(actualAttackTransform.position, actualAttackParams);
        Gizmos.DrawWireCube(attackPoint.position, punchAttackBoxParams);
        Gizmos.DrawWireCube(kickTransform.position, kickAttackBoxParams);
        Gizmos.DrawWireCube(crouchKickTransform.position, crouchKickAttackBoxParams);
    }

}
