using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatedPlatformMovement : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float movementSpeed = 3f;

    private Vector3 movementVec;
    private Transform otherBody;

    [SerializeField] private int startIndex = 0;

    private int waypointIndex = 0;


    void Start()
    {
        waypointIndex = startIndex;
        UpdateMovementVector();
    }

    private void FixedUpdate()
    {
        transform.position += movementVec * Time.deltaTime;

    }


    private void UpdateMovementVector()
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
        if (collision.tag == "platformReversePoint" && collision.transform.IsChildOf(transform.parent))
        {
            UpdateMovementVector();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            otherBody = collision.transform;
            if (otherBody != null)
            {
                otherBody.parent = this.transform;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform == otherBody && otherBody != null
            && (otherBody.GetComponent<PlayerController>().GetIsFalling() || otherBody.GetComponent<PlayerController>().GetIsJumping()))
        {
            var otherMovementVector = collision.gameObject.GetComponent<Rigidbody2D>().velocity + new Vector2(movementVec.x, movementVec.y);
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = otherMovementVector;
            otherBody.parent = null;
            otherBody = null;
        }
    }
}
