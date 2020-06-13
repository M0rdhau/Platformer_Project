using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeedMax = 3f;
    [SerializeField] float runSpeedMax = 6f;
    [SerializeField] float acceleration = 0.2f;
    Vector2 accelerationVector;
    bool isRunning = false;
    [SerializeField] float walkSpeed = 0f;


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
        HandleVerticalMovement();
        walkSpeed = _rigidBody.velocity.x;
    }

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            HandleHorizontalMovement(1f);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            HandleHorizontalMovement(-1f);
        }
        else
        {
            _rigidBody.velocity = new Vector2(0, 0);
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isWalking", false);
        }

    }

    void HandleAcceleration()
    {
        Vector2 vel = _rigidBody.velocity;
        vel += accelerationVector;
        if (!isRunning)
        {
            vel.x = Mathf.Clamp(vel.x, -walkSpeedMax, walkSpeedMax);
        }
        else
        {
            vel.x = Mathf.Clamp(vel.x, -runSpeedMax, runSpeedMax);
        }

        _rigidBody.velocity = vel;
    }

    private void HandleVerticalMovement()
    {

    }

    private void HandleHorizontalMovement(float a)
    {
            accelerationVector.x = a*acceleration;
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isWalking", true);
            if (isRunning)
            {
                _animator.SetBool("isRunning", true);
            }

            if (accelerationVector.x > 0)
            {
                _renderer.flipX = false;
            }
            else if (accelerationVector.x < 0)
            {
                _renderer.flipX = true;
            }
        


        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }


}
