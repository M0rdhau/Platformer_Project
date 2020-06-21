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
    GameObject Platform;
    Rigidbody2D rBody;
    Rigidbody2D otherBody;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponentInChildren<Rigidbody2D>();
        SetMovementVector();
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
        otherBody = collision.gameObject.GetComponentInParent<Rigidbody2D>();
        Vector2 colVelocity = otherBody.velocity;
        otherBody.velocity = colVelocity + rBody.velocity;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 colVelocity = otherBody.velocity;
        otherBody.velocity = colVelocity + rBody.velocity;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponentInParent<Rigidbody2D>() == otherBody)
        {
            otherBody = new Rigidbody2D();
        }
    }

    private void ReverseMovement()
    {
        switch (direction)
        {
            case ReversePositions.X:
                movementVec.x = -movementVec.x;
                SetMovementVector();
                break;
            case ReversePositions.Y:
                movementVec.y = -movementVec.y;
                SetMovementVector();
                break;
            case ReversePositions.Both:
                movementVec = -movementVec;
                SetMovementVector();
                break;
        }
        
    }

    private void SetMovementVector()
    {
        rBody.velocity = movementVec;
    }
}
