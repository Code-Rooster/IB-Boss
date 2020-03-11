using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : MonoBehaviour
{
    private CameraShake camShake;

    private GameObject outline;

    private Rigidbody2D rb;

    public float shakeMagnitude;
    public int shakeIterations;
    public float timePerShakeCycle;
    public float shakeRoughness;

    public bool isGrabable = true;
    public bool beingSelected = false;
    public bool letGo = false;

    private void Start()
    {
        camShake = Camera.main.GetComponent<CameraShake>();

        outline = GameObject.Find("Outline");

        rb = this.gameObject.GetComponent<Rigidbody2D>();

        outline.SetActive(false);
    }

    private void Update()
    {
        if (beingSelected)
        {
            outline.SetActive(true);
        }
        else
        {
            outline.SetActive(false);
        }

        if (letGo)
        {
            if (Vector2.Distance(rb.velocity, Vector2.zero) < 0.3f)
            {
                letGo = false;
            }
        }

        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -20, 20), Mathf.Clamp(rb.velocity.y, -20, 20));
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        EnemyHealth eH = col.gameObject.GetComponent<EnemyHealth>();

        if (eH != null && letGo)
        {
            if (rb.velocity.x > 10f || rb.velocity.y > 10f)
            {
                eH.TakeDamage(4f);
            }
            else if (rb.velocity.x > 5f || rb.velocity.y > 5f)
            {
                eH.TakeDamage(3f);
            }
            else if (rb.velocity.x > 2.5f || rb.velocity.y > 2.5f)
            {
                eH.TakeDamage(2f);
            }
            else if (rb.velocity.x > 0.5f || rb.velocity.y > 0.5f)
            {
                eH.TakeDamage(1f);
            }
        }

        else if (col.gameObject.tag == "The Entertainer" && letGo)
        {
            EntertainerHealth enH = GameObject.Find("The Entertainer").GetComponent<EntertainerHealth>();

            print("I've been hit! Velocity: " + gameObject.GetComponent<Rigidbody2D>().velocity);

            if (rb.velocity.x > 10f || rb.velocity.y > 10f)
            {
                enH.TakeDamage(20f);
            }
            else if (rb.velocity.x > 5f || rb.velocity.y > 5f)
            {
                enH.TakeDamage(10f);
            }
            else if (rb.velocity.x > 0.05f || rb.velocity.y > 0.05f)
            {
                enH.TakeDamage(5f);
            }
        }

        else if ((GlobalManager.Instance.letGoCollision & 1 << col.gameObject.layer) == 1 << col.gameObject.layer)
        {
            if (letGo)
            {
                camShake.StartShake(7, 1f);

                letGo = false;
            }
        }

        if (col.collider.tag == "Player" && letGo)
        {
            letGo = false;
        }
        else if (col.collider.tag == "Enemy")
        {
            Vector2 kbDir = rb.velocity * -100;

            //col.gameObject.GetComponent<EnemyHealth>().Knockback(kbDir);
        }
    }
}
