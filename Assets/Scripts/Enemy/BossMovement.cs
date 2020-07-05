using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : EnemyMovement
{
    float flipXHitbox = 0.3f;
    float nonFlipXHitbox = -1.2f;


    protected override void Update()
    {
        body.velocity = movementVec;
        if (health.IsDead())
        {
            movementVec = new Vector2(0, 0);
        }
    }

    public override void ReverseDirection(Collider2D collision)
    {
        var collider = GetComponent<BoxCollider2D>();

        if (collision.tag == "BossReflectorHorizontal")
        {
            Debug.Log("Collision, movement vector: " + movementVec);
            movementVec.y = -movementVec.y;
            SetMovementVector(movementVec);
            Debug.Log("Movement vector after collision: " + movementVec);
            Debug.Log("rbody movement: " + body.velocity);
            GetComponent<BossCombat>().Bounce();
        }
        else if (collision.tag == "BossReflectorVertical")
        {
            movementVec.x = -movementVec.x;
            render.flipX = !render.flipX;
            if (render.flipX)
            {
                var colliderVector = collider.offset;
                colliderVector.x = flipXHitbox;
                collider.offset = colliderVector;
            }
            else
            {
                var colliderVector = collider.offset;
                colliderVector.x = nonFlipXHitbox;
                collider.offset = colliderVector;
            }
            SetMovementVector(movementVec);
            GetComponent<BossCombat>().Bounce();
        }
    }
}
