using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, ISaveable
{
    [SerializeField] float MoveSpeed = 2f;
    [SerializeField] Vector2 movementVec;
    [SerializeField] float damagedTimeFrame = 0.5f;
    float timeLastDamaged = 0;
    Rigidbody2D body;
    SpriteRenderer render;
    Health health;

    private void Start()
    {
        health = GetComponent<Health>();
        body = GetComponent<Rigidbody2D>();
        movementVec = MoveSpeed * Vector2.left;
        render = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        body.velocity = new Vector2(movementVec.x, body.velocity.y);
        if (health.IsDead())
        {
            movementVec = new Vector2(0, 0);
        }
    }

    public IEnumerator Damaged()
    {
        if (Time.time - timeLastDamaged > damagedTimeFrame)
        {
            movementVec.x = 0;
            yield return new WaitForSeconds(damagedTimeFrame);
            UnDamaged();
        }
    }

    private void UnDamaged()
    {
        if (render.flipX)
        {
            movementVec = MoveSpeed * Vector2.right;
        }
        else
        {
            movementVec = MoveSpeed * Vector2.left;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyLedgeLeft")
        {
            movementVec = MoveSpeed * Vector2.right;
            render.flipX = true;
        }
        else if (collision.tag == "EnemyLedgeRight")
        {
            movementVec = MoveSpeed * Vector2.left;
            render.flipX = false;
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
