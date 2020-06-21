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

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
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
        var otherRB = collision.gameObject.GetComponent<Rigidbody2D>();
        Vector2 colVelocity = otherRB.velocity;
        otherRB.velocity = colVelocity + rBody.velocity;
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
                movementVec.x = -movementVec.y;
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
