using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAI : MonoBehaviour
{
    public float force;

    public bool beenGrabbed = false;

    private void Update()
    {
        if (GameObject.Find("Player").transform.Find("Orbit").GetComponent<Telekinesis>().inOrbit == transform)
        {
            beenGrabbed = true;
        }

        if (!beenGrabbed)
        {
            Vector2 dir = GameObject.Find("Player").transform.position - transform.position;

            gameObject.GetComponent<Rigidbody2D>().AddForce(dir * force);
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2
            (
                Mathf.Clamp(gameObject.GetComponent<Rigidbody2D>().velocity.x, -5, 5),
                Mathf.Clamp(gameObject.GetComponent<Rigidbody2D>().velocity.y, -5, 5)
            );

            transform.up = -gameObject.GetComponent<Rigidbody2D>().velocity;
        }
    }
}
