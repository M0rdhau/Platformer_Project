using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : EnemyMovement
{
    float flipXHitbox = 0.3f;
    float nonFlipXHitbox = -1.2f;


    protected override void Update()
    {
        //body.velocity = movementVec;
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
            movementVec = body.velocity;
            movementVec.y = -body.velocity.y;
            SetMovementVector(movementVec);
        }
        else if (collision.tag == "BossReflectorVertical")
        {
            movementVec = body.velocity;
            movementVec.x = -body.velocity.x;
            SetMovementVector(movementVec);
        }

        if (body.velocity.x > 0)
        {
            render.flipX = true;
        }
        else
        {
            render.flipX = false;
        }
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
        GetComponent<BossCombat>().Bounce();
    }
}
