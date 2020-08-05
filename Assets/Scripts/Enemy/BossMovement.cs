using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : EnemyMovement
{
    private float flipXHitbox = 0.3f;
    private float nonFlipXHitbox = -1.2f;

    private BossCombat combat;
    private BoxCollider2D boxCollider;

    protected override void OnAwake()
    {
        combat = GetComponent<BossCombat>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

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
        if (!homingIn)
        {
            if (collision.tag == "BossReflectorHorizontal")
            {
                movementVec = body.velocity;
                movementVec.y = -body.velocity.y;
                SetMovementVector(movementVec);
                combat.Bounce();
            }
            else if (collision.tag == "BossReflectorVertical")
            {
                movementVec = body.velocity;
                movementVec.x = -body.velocity.x;
                SetMovementVector(movementVec);
                combat.Bounce();
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
                var colliderVector = boxCollider.offset;
                colliderVector.x = flipXHitbox;
                boxCollider.offset = colliderVector;
            }
            else
            {
                var colliderVector = boxCollider.offset;
                colliderVector.x = nonFlipXHitbox;
                boxCollider.offset = colliderVector;
            }
        }
        
        
    }
}
