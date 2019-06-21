using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector2 vel = Vector2.zero;

    public GameObject player;

    public Animator anim;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        anim = player.GetComponent<Animator>();
    }

    void Update()
    {
        vel = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (vel.x > 0)
        {
            if (vel.y == 0);
        }
    }
}
