using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float boostSpeed;

    public Vector2 movement;

    public bool isMoving;
    public bool canMove = true;
    public bool isBoosting = false;
    private bool resetSpeed = false;

    public Rigidbody2D rb;

    public Animator anim;

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

            
        }
        else if (!canMove && !resetSpeed)
        {
            rb.velocity = Vector2.zero;

            resetSpeed = true;
        }

        if (isBoosting)
        {
            if (rb.velocity.x == 0 && rb.velocity.y != 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, (rb.velocity.y / Mathf.Abs(rb.velocity.y)) * (Mathf.Sqrt(2) * boostSpeed), 0);
            }
            else if (rb.velocity.x != 0 && rb.velocity.y == 0)
            {
                rb.velocity = new Vector3((rb.velocity.x / Mathf.Abs(rb.velocity.x)) * (Mathf.Sqrt(2) * boostSpeed), rb.velocity.y, 0);
            }
            else if (rb.velocity.x != 0 || rb.velocity.y != 0)
            {
                rb.velocity = new Vector3((rb.velocity.x / Mathf.Abs(rb.velocity.x)) * boostSpeed, (rb.velocity.y / Mathf.Abs(rb.velocity.y)) * boostSpeed, 0);
            }
        }
    }
}
