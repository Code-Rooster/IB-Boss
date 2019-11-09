using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 movement;

    public float speed;
    public float boostSpeed;
    public float boostTimer;
    public float timeTilBoost;
    public float maxBoostTimer;
    public float boostWaitTime;

    public bool isMoving;
    public bool canBoost = true;
    public bool isBoosting = false;

    public Rigidbody2D rb;

    public Animator anim;

    public bool canMove = true;
    private bool resetSpeed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal") * speed, Input.GetAxisRaw("Vertical") * speed);

        if (movement != Vector2.zero)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (canMove)
        {
            rb.velocity = movement;

            anim.SetFloat("xMovement", movement.x);
            anim.SetFloat("yMovement", movement.y);

            resetSpeed = false;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                print("Space Pressed");
                if (canBoost)
                {
                    print("Boost Initiated");
                    canBoost = false;
                    isBoosting = true;
                    boostTimer = 0;
                }
            }
        }
        else if (!canMove && !resetSpeed)
        {
            rb.velocity = Vector2.zero;

            resetSpeed = true;
        }

        if (isBoosting)
        {
            boostTimer += Time.fixedUnscaledDeltaTime;

            if (canBoost)
            {
                Debug.LogError("CanBoost while boosting");
            }

            if (boostTimer <= maxBoostTimer)
            {
                if (rb.velocity.x == 0 && rb.velocity.y != 0)
                {
                    rb.velocity = new Vector3(rb.velocity.x, (rb.velocity.y / Mathf.Abs(rb.velocity.y)) * (Mathf.Sqrt(2) * boostSpeed), 0);
                }
                else if (rb.velocity.x != 0 && rb.velocity.y == 0)
                {
                    rb.velocity = new Vector3((rb.velocity.x / Mathf.Abs(rb.velocity.x)) * (Mathf.Sqrt(2) * boostSpeed), rb.velocity.y, 0);
                }
                else
                {
                    rb.velocity = new Vector3((rb.velocity.x / Mathf.Abs(rb.velocity.x)) * boostSpeed, (rb.velocity.y / Mathf.Abs(rb.velocity.y)) * boostSpeed, 0);
                }
            }

            else
            {
                isBoosting = false;
                canBoost = false;
                timeTilBoost = 0;
            }
        }

        if (timeTilBoost <= boostWaitTime && !isBoosting)
        {
            timeTilBoost += Time.fixedDeltaTime;
        }
        else if (timeTilBoost > boostWaitTime && !isBoosting)
        {
            canBoost = true;
        }
    }
}
