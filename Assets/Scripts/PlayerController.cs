using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeedMax = 3f;
    [SerializeField] float runSpeedMax = 6f;
    [SerializeField] float acceleration = 0.2f;
    [SerializeField] float jumpVelocity = 6f;

    float fallTime;
    //vertical velocity necessary to roll
    [SerializeField] float rollTime = 0.8f;
    Vector2 accelerationVector;
    bool isRunning = false;
    bool isClimbing = false;
    bool isCrouching = false;
    bool isJumping = false;
    bool isFalling = false;

    //parameters for monitoring
    [SerializeField] float walkSpeed = 0f;
    [SerializeField] float verticalSpeed = 0f;


    SpriteRenderer _renderer;
    Animator _animator;
    Collider2D _collider;
    Rigidbody2D _rigidBody;


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleAcceleration();
        HandleInput();
        CheckForFalling();
        walkSpeed = _rigidBody.velocity.x;
        verticalSpeed = _rigidBody.velocity.y;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 && (isFalling || isJumping))
        {
            isFalling = false;
            isJumping = false;
            _animator.SetBool("isFalling", false);
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


    void HandleInput()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            HandleHorizontalMovement(Input.GetAxis("Horizontal"));
        }
        else
        {
            Vector2 vel = _rigidBody.velocity;
            vel.x = 0;
            _rigidBody.velocity = vel;
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isWalking", false);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (Input.GetAxis("Jump") > 0)
        {
            Jump();
        }


        if (Input.GetAxis("Vertical") != 0)
        {
            HandleVerticalMovement(Input.GetAxis("Vertical"));
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


    private void CheckForFalling()
    {
        if (_rigidBody.velocity.y < -1*Mathf.Epsilon && !isTouchingGround() && !isFalling)
        {
            isJumping = false;
            isFalling = true;
            _animator.SetBool("isFalling", true);
            fallTime = Time.time;
        }
    }



    private void HandleVerticalMovement(float axisThrow)
    {
        

    }

    private void Jump()
    {
        if (isTouchingGround())
        {
            var jumpVec = _rigidBody.velocity;
            jumpVec.y = jumpVelocity;
            _rigidBody.velocity = jumpVec;
            isJumping = true;
            _animator.SetTrigger("jump");
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

    private bool isTouchingGround()
    {
        return _collider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }


}
