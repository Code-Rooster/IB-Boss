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
        float force = Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.y, 2));

        force = Mathf.Clamp(force, 0, 2.75f);

        EnemyHealth eH = col.gameObject.GetComponent<EnemyHealth>();

        if (eH != null && letGo)
        {
            eH.TakeDamage(force);
        }

        if ((GlobalManager.Instance.letGoCollision & 1 << col.gameObject.layer) == 1 << col.gameObject.layer)
        {
            if (letGo)
            {
                camShake.StopAllCoroutines();
                camShake.StartCoroutine(camShake.ShakeCam(shakeMagnitude * force, shakeIterations, timePerShakeCycle, shakeRoughness));

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
