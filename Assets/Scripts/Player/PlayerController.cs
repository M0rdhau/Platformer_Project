using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ISaveable
{

    [Header("Movement")]

    [SerializeField] float walkSpeedMax = 3f;
    [SerializeField] float runSpeedMax = 6f;
    [SerializeField] float acceleration = 0.2f;
    [SerializeField] float jumpVelocity = 8f;
    [SerializeField] float climbSpeed = 3f;
    //vertical velocity necessary to roll
    [SerializeField] float rollTime = 0.8f;
    [SerializeField] float climbDelayTime = 0.2f;

    [SerializeField] float knockbackX = 1f;
    [SerializeField] float knockbackY = 0.5f;

    [SerializeField] Vector2 accelerationVector;
    float fallTime;
    float jumpTime;
    bool isCrouching = false;
    bool isRunning = false;
    bool isClimbing = false;
    bool isJumping = false;
    bool isFalling = false;
    bool hasStopped = true;

    bool canJumpOrFall = false;

    bool areControlsEnabled = true;

    //TODO: get these values at runtime from colliders
    float playerColliderHeight = 0.75f;
    float playerColliderWidth = 0.72f;

    SpriteRenderer _renderer;
    Animator _animator;
    Collider2D feetCollider;
    Collider2D handsCollider;
    Rigidbody2D _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        feetCollider = GetComponent<CapsuleCollider2D>();
        handsCollider = GetComponent<BoxCollider2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleAcceleration();
        if (areControlsEnabled) { HandleMovementInput(); }
        CheckForFalling();
    }

    public void KnockBack(bool knockedRight)
    {
        _animator.SetBool("knockedBack", true);
        areControlsEnabled = false;
        accelerationVector = new Vector2(0, 0);
        if (knockedRight)
        {
            _rigidBody.velocity = new Vector2(knockbackX, knockbackY);
        }
        else
        {
            _rigidBody.velocity = new Vector2(-knockbackX, knockbackY);
        }
    }

    public void EndKnockBack()
    {
        _animator.SetBool("knockedBack", false);
        areControlsEnabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Land();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Land();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        canJumpOrFall = true;
    }

    private void Land()
    {
        if ((isTouchingGround() || isTouchingLadders()) && (isFalling || isJumping))
        {
            canJumpOrFall = false;
            isFalling = false;
            isJumping = false;
            _animator.SetBool("isFalling", false);
            _animator.SetBool("kickAerial", false);
            if (isTouchingGround())
            {
                if (Time.time - fallTime < rollTime)
                {
                    _animator.SetTrigger("landed_Noroll");
                }
                else
                {
                    _animator.SetTrigger("landed");
                }
            }
        }
    }

    public void MoveToLedge()
    {
        var ledgePos = transform.position;
        ledgePos.y += playerColliderHeight;
        if (_renderer.flipX)
        {
            ledgePos.x -= playerColliderWidth;
        }
        else
        {
            ledgePos.x += playerColliderWidth;
        }
        transform.position = ledgePos;
        _animator.SetBool("isLedging", false);
        _rigidBody.gravityScale = 0;
        _animator.speed = 0;
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

    #region Movement

    void HandleMovementInput()
    {
        if (_animator.GetBool("isLedging") && _animator.speed == 0)
        {
            if (Input.anyKey)
            {
                MoveToLedge();
            }
        }

        HandleClimb();

        if (Input.GetKey(KeyCode.D))
        {
            HandleHorizontalMovement(1f);
            hasStopped = false;
        }else if (Input.GetKey(KeyCode.A))
        {
            HandleHorizontalMovement(-1f);
            hasStopped = false;
        }
        else if(!hasStopped)
        {
            Vector2 vel = _rigidBody.velocity;
            vel.x = 0;
            _rigidBody.velocity = vel;
            accelerationVector = new Vector2(0, 0);
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isWalking", false);
            hasStopped = true;
        }

        if (!hasStopped)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void HandleClimb()
    {
        float axisThrow = Input.GetAxis("Vertical");
        if (axisThrow != 0)
        {
            jumpTime = 0;
        }
        if (isTouchingLadders())
        {
            isFalling = false;
            isJumping = false;
            _animator.SetBool("isFalling", false);
            _animator.SetBool("kickAerial", false);
            if (!isTouchingGround() && isTouchingLadders())
            {
                _rigidBody.gravityScale = 0;
                isClimbing = true;
                _animator.SetBool("isClimbing", isClimbing);
            }

            if (axisThrow != 0)
            {
                _animator.speed = 1;
                var climbVec = _rigidBody.velocity;
                climbVec.y = climbSpeed * axisThrow;
                _rigidBody.velocity = climbVec;

            }
            else if (isClimbing)
            {
                _animator.speed = 0;
                var climbVec = _rigidBody.velocity;
                climbVec.y = 0;
                _rigidBody.velocity = climbVec;
            }
        }
        else
        {
            _rigidBody.gravityScale = 1;
            _animator.speed = 1;
            isClimbing = false;
            _animator.SetBool("isClimbing", isClimbing);
        }

    }

    private void CheckForFalling()
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && handsCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            canJumpOrFall = true;
        }
        if (_rigidBody.velocity.y < -1 * Mathf.Epsilon && !isTouchingGround() && !isFalling && !isClimbing && canJumpOrFall)
        {
            isJumping = false;
            isFalling = true;
            _animator.SetBool("isFalling", true);
            fallTime = Time.time;
        }
    }

    private void Jump()
    {
        if (isTouchingGround() || isTouchingLadders())
        {
            var jumpVec = _rigidBody.velocity;
            jumpVec.y = jumpVelocity;
            _rigidBody.velocity = jumpVec;
            isJumping = true;
            _animator.SetTrigger("jump");
            jumpTime = Time.time;
        }
    }

    private void HandleHorizontalMovement(float axisThrow)
    {
        accelerationVector.x = axisThrow * acceleration;
        if (isTouchingGround())
        {
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isWalking", true);
            if (isRunning)
            {
                _animator.SetBool("isRunning", true);
            }
        }

        if (accelerationVector.x > 0)
        {
            _renderer.flipX = false;
        }
        else if (accelerationVector.x < 0)
        {
            _renderer.flipX = true;
        }
    }

    void HandleAcceleration()
    {
        Vector2 vel = _rigidBody.velocity;

        if (!isRunning)
        {
            vel += accelerationVector;
            vel.x = Mathf.Clamp(vel.x, -walkSpeedMax, walkSpeedMax);
        }
        else
        {
            vel += accelerationVector * 2;
            vel.x = Mathf.Clamp(vel.x, -runSpeedMax, runSpeedMax);
        }

        _rigidBody.velocity = vel;
    }

    private bool isTouchingLadders()
    {
        if (Time.time - jumpTime <= climbDelayTime) return false;
        return handsCollider.IsTouchingLayers(LayerMask.GetMask("Ladders"));
    }

    private bool isTouchingLedges()
    {
        return handsCollider.IsTouchingLayers(LayerMask.GetMask("Ledges"));
    }

    private bool isTouchingGround()
    {
        return feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Ledges"));
    }
    #endregion
}
