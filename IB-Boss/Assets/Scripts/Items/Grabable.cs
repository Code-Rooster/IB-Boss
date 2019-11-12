using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : MonoBehaviour
{
    private CameraShake camShake;

    private GameObject outline;

    private Rigidbody2D rb;

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
            if (Vector2.Distance(rb.velocity, Vector2.zero) < 0.1f)
            {
                letGo = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        float force = Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.y, 2));

        if ((GlobalManager.Instance.letGoCollision & 1 << col.gameObject.layer) == 1 << col.gameObject.layer)
        {
            if (letGo)
            {
                camShake.StartCoroutine(camShake.ShakeCam(force * (rb.mass / 3), 6, 0.05f));

                letGo = false;
            }
        }
    }
}
