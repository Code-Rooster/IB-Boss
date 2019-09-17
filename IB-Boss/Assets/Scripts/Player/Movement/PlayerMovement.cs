using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 movement;

    public float speed;

    public bool isMoving;

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
        }
        else if (!canMove && !resetSpeed)
        {
            rb.velocity = Vector2.zero;

            resetSpeed = true;
        }
    }
}
