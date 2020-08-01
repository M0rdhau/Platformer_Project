using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    enum ReversePositions
    {
        X,
        Y,
        Both
    };


    [SerializeField] Vector2 movementVec = new Vector2(3, 0);
    [SerializeField] ReversePositions direction = ReversePositions.X;
    private Collider2D coll;
    Rigidbody2D rBody;
    Transform otherBody;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider2D>();
        rBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        transform.position += (new Vector3(movementVec.x, movementVec.y, 0)) * Time.deltaTime;
        if (otherBody != null && !coll.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            if (otherBody.GetComponent<PlayerController>().GetIsFalling() || otherBody.GetComponent<PlayerController>().GetIsJumping())
            {
                otherBody.parent = null;
                otherBody = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "platformReversePoint")
        {
            ReverseMovement();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            otherBody = collision.transform;
            if (otherBody)
            {
                otherBody.parent = this.transform;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform == otherBody 
            && (otherBody.GetComponent<PlayerController>().GetIsFalling() || otherBody.GetComponent<PlayerController>().GetIsJumping()))
        {
            var otherMovementVector = collision.gameObject.GetComponent<Rigidbody2D>().velocity + new Vector2(movementVec.x, movementVec.y);
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = otherMovementVector;
            otherBody.parent = null;
            otherBody = null;
        }
    }

    private void ReverseMovement()
    {
        switch (direction)
        {
            case ReversePositions.X:
                movementVec.x = -movementVec.x;
                break;
            case ReversePositions.Y:
                movementVec.y = -movementVec.y;
                break;
            case ReversePositions.Both:
                movementVec = -movementVec;
                break;
        }
    }

}
