using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, ISaveable
{
    //[SerializeField] protected float MoveSpeed = 2f;
    [SerializeField] protected Vector2 movementVec;
    [SerializeField] float damagedTimeFrame = 1f;
    [SerializeField] float knockbackX = 3f;
    [SerializeField] float knockbackY = 1f;
    public bool homingIn { get; set; }
    float timeLastDamaged = 0;
    protected Rigidbody2D body;
    protected SpriteRenderer render;
    protected Health health;

    private void Awake()
    {
        homingIn = false;
        health = GetComponent<Health>();
        body = GetComponent<Rigidbody2D>();
        render = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        body.velocity = new Vector2(movementVec.x, body.velocity.y);
        if (health.IsDead())
        {
            movementVec = new Vector2(0, 0);
        }
    }

    public IEnumerator Damaged()
    {
        Vector2 rememberVec = movementVec;
        SetMovementVector(Vector2.zero);
        yield return new WaitForSeconds(damagedTimeFrame);
        movementVec = rememberVec;
    }

    public IEnumerator KnockBack(bool knockedRight)
    {
        Vector2 rememberVec = movementVec;
        if (knockedRight)
        {
            movementVec = new Vector2(knockbackX, knockbackY);
        }
        else
        {
            movementVec = new Vector2(-knockbackX, knockbackY);
        }
        yield return new WaitForSeconds(damagedTimeFrame);
        movementVec = rememberVec;
    }

    
    private void UnDamaged(Vector2 remVec)
    {
        if (render.flipX)
        {
            movementVec = remVec;
        }
        else
        {
            movementVec = remVec;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
            ReverseDirection(collision);
        
    }

    public void SetMovementVector(Vector2 movVec)
    {
        movementVec = movVec;
        body.velocity = movVec;
    }

    public virtual void ReverseDirection(Collider2D collision)
    {
        if (!homingIn)
        {
            if (collision.tag == "EnemyLedgeLeft")
            {
                movementVec = Mathf.Abs(movementVec.x) * Vector2.right;
                render.flipX = true;
            }
            else if (collision.tag == "EnemyLedgeRight")
            {
                movementVec = Mathf.Abs(movementVec.x) * Vector2.left;
                render.flipX = false;
            }
        }

    }

    public object CaptureState()
    {
        return new SerializableVector(transform.position);
    }

    public void RestoreState(object state)
    {
        SerializableVector vec = state as SerializableVector;
        transform.position = vec.GetVector();
    }
}
