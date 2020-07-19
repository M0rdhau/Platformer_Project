using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpadtedPlatformMovement : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float movementSpeed = 3f;

    Vector2 movementVec;
    Transform otherBody;

    int waypointIndex = 0;


    void Start()
    {
        UpdateMovementVector();
    }

    private void FixedUpdate()
    {
        transform.position += (new Vector3(movementVec.x, movementVec.y, 0)) * Time.deltaTime;
    }


    void UpdateMovementVector()
    {
        movementVec = (waypoints[waypointIndex].position - transform.position).normalized * movementSpeed;
        if (waypointIndex == waypoints.Length - 1)
        {
            waypointIndex = 0;
        }
        else
        {
            waypointIndex++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "platformReversePoint")
        {
            UpdateMovementVector();
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
        if (collision.transform == otherBody)
        {
            otherBody.parent = null;
            otherBody = null;
        }
    }
}
