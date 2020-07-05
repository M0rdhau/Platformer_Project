using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : GhostCombat
{

    [SerializeField] int maxBounces = 2;
    [SerializeField] float maxSpeed = 4f;
    [SerializeField] float minSpeed = 1f;
    int timesBounced = 0;

    // Start is called before the first frame update
    void Start()
    {
        timesBounced = 0;
        player = FindObjectOfType<PlayerController>().transform;
        health = GetComponent<EnemyHealth>();
        _animator = GetComponent<Animator>();
        movement = GetComponent<BossMovement>();
        breathOffsetX = Mathf.Abs(BreathTransform.position.x - transform.position.x);
        _animator.SetTrigger("Transform");
    }

    private void Update()
    {
        if (player != null) InitiateAttack();
    }

    protected override void OnAttack()
    {
        _animator.SetTrigger("Attack");
    }

    private void BreatheOut()
    {
        var breath = Instantiate(breathPrefab, BreathTransform.position, breathRotation);
        breath.GetComponent<Breath>().SetDamage(breathDamage);
        breath.GetComponent<Animator>().SetTrigger("Red");
    }

    protected override void OnAttackCompleted()
    {
        return;
    }

    //called after the breath animation is finished
    private void StartMovement()
    {
        float randomX = UnityEngine.Random.Range(0f, 1f);
        float randomY = (float)Math.Sqrt(1 - randomX * randomX);
        float randomSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        var randomVec = new Vector2(randomX, randomY);
        movement.SetMovementVector(randomSpeed * randomVec);
    }

    public void Bounce()
    {
        timesBounced++;
        if (timesBounced >= maxBounces)
        {
            timesBounced = 0;
            isAttacking = false;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(BreathTransform.position, breathRadius);
    }
}
