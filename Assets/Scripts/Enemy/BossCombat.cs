using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : GhostCombat
{

    [SerializeField] int maxBounces = 2;
    [SerializeField] float maxSpeed = 4f;
    [SerializeField] float minSpeed = 1f;
    [SerializeField] GameObject ringFire;
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] Transform[] fireballPoints;
    [SerializeField] float fireballRotationSpeed = 30f;
    [SerializeField] float fireballOffset = 1f;
    [SerializeField] float fireballDelayTime = 3f;
    [SerializeField] float fireballSpeed = 3f;
    int timesBounced = 0;

    bool ringActivated = false;
    bool fireBallThrow = false;

    IEnumerator stayWithPlayer;

    // Start is called before the first frame update
    void Start()
    {
        timesBounced = 0;
        ringFire.SetActive(false);
        player = FindObjectOfType<PlayerController>().transform;
        health = GetComponent<BossHealth>();
        _animator = GetComponent<Animator>();
        movement = GetComponent<BossMovement>();
        breathOffsetX = Mathf.Abs(BreathTransform.position.x - transform.position.x);
        _animator.SetTrigger("Transform");
    }

    private void Update()
    {
        if (!health.IsDead())
        {
            if (player != null)
            {
                InitiateAttack();
            }

            if (health.GetHealth() <= health.GetMaxHealth() * 2 / 3)
            {
                HandleFireball();
            }

            if (health.GetHealth() <= health.GetMaxHealth() / 3 && !ringActivated)
            {
                HandleRingOfFireUpgrade();
            }
        }
    }

    private void HandleFireball()
    {
        CheckFireballPositions();
        foreach (Transform point in fireballPoints)
        {

            if (point.childCount < 1)
            {
                StartCoroutine(LaunchFireball(point));
            }
        }
    }

    private void notAttacking()
    {
        isAttacking = false;
    }

    #region powerups
    private IEnumerator LaunchFireball(Transform point)
    {
        GameObject fireball = CreateFireball(point);
        yield return new WaitForSeconds(fireballDelayTime);
        //check if fireball exists - player might have collided with it already
        if (fireball)
        {
            fireball.GetComponent<Fireball>().isBossFireball = false;
            fireball.transform.parent = null;
            Vector3 directionVector = (player.position - point.position).normalized;
            fireball.GetComponent<Fireball>().SetMoveVector(directionVector * fireballSpeed);
        }
    }

    private GameObject CreateFireball(Transform point)
    {
        var instantPoint = point.position;
        instantPoint.x -= fireballOffset;
        var fireball = Instantiate(fireballPrefab, instantPoint, point.rotation, point);
        fireball.GetComponent<Fireball>().isBossFireball = true;
        fireball.GetComponent<Fireball>().rotationSpeed = fireballRotationSpeed;
        return fireball;
    }

    private void CheckFireballPositions()
    {
        var _renderer = GetComponentInChildren<SpriteRenderer>();
        float diffOne = Mathf.Abs(fireballPoints[0].position.x - transform.position.x);
        float diffTwo = Mathf.Abs(fireballPoints[1].position.x - transform.position.x);
        if (_renderer.flipX)
        {
            fireballPoints[0].position = new Vector2(transform.position.x + diffOne, fireballPoints[0].position.y);
            fireballPoints[1].position = new Vector2(transform.position.x - diffTwo, fireballPoints[1].position.y);
        }
        else
        {
            fireballPoints[0].position = new Vector2(transform.position.x - diffOne, fireballPoints[0].position.y);
            fireballPoints[1].position = new Vector2(transform.position.x + diffTwo, fireballPoints[1].position.y);
        }
    }

    private void HandleRingOfFireUpgrade()
    {
        ringFire.SetActive(true);
    }

    #endregion

    protected override void OnAttack()
    {
        if (stayWithPlayer != null) { stayWithPlayer = null; }
        movement.SetMovementVector(Vector2.zero);
        _animator.SetTrigger("Attack");
        stayWithPlayer = StayWithPlayer();
        StartCoroutine(stayWithPlayer);
    }

    private IEnumerator StayWithPlayer()
    {
        Vector3 offsetFromPlayer = player.position - transform.position;
        while (true)
        {
            movement.SetMovementVector(Vector2.zero);
            transform.position = player.position - offsetFromPlayer;
            yield return null;
        }
    }

    private void BreatheOut()
    {
        if (stayWithPlayer != null) { StopCoroutine(stayWithPlayer); }
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
