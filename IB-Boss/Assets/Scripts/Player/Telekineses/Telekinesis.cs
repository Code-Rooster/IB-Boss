using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telekinesis : MonoBehaviour
{
    public CameraShake camShake;

    public Transform inOrbit;
    public GameObject selected;

    public float maxRadius;
    public float maxAcceleration;
    public float minAcceleration;
    public float accelerationMult;

    private float orbitAcceleration;
    private float directionMult;

    public List<GameObject> grabable = new List<GameObject>();

    public Vector3 mousePos;
    public Vector3 dir;

    RaycastHit2D lastHit;

    public LayerMask orbitLayers;

    public bool canGrab = true;

    private void Start()
    {
        camShake = Camera.main.GetComponent<CameraShake>();
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Debug.DrawRay(transform.position, mousePos - transform.position);

        if (canGrab)
        {
            dir = (mousePos - transform.position);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 10, orbitLayers);

            if (lastHit.collider != null && hit.collider != lastHit.collider)
            {
                lastHit.collider.gameObject.GetComponent<Grabable>().beingSelected = false;
            }

            if (hit.collider != null && grabable.Contains(hit.collider.gameObject))
            {
                selected = hit.collider.gameObject;
                selected.GetComponent<Grabable>().beingSelected = true;
            }
            else if (hit.collider == null)
            {
                selected = null;
            }

            lastHit = hit;

            if (grabable.Count == 1)
            {
                selected = grabable[0];
                selected.GetComponent<Grabable>().beingSelected = true;
            }
        }

        if (selected != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                inOrbit = selected.transform;

                inOrbit.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                orbitAcceleration = minAcceleration;

                directionMult = -1;

                selected.GetComponent<Grabable>().beingSelected = false;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                inOrbit = selected.transform;

                inOrbit.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                orbitAcceleration = minAcceleration;

                directionMult = 1;

                selected.GetComponent<Grabable>().beingSelected = false;
            }
        }

        if (inOrbit != null)
        {
            canGrab = false;

            Vector3 target = inOrbit.position;

            target -= transform.position;

            target.z = 0;

            float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg - 270;

            inOrbit.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            inOrbit.gameObject.GetComponent<Rigidbody2D>().AddForce(inOrbit.right * directionMult * orbitAcceleration);

            inOrbit.position = (inOrbit.position - this.gameObject.transform.position).normalized * maxRadius + this.gameObject.transform.position;

            if (Input.GetKeyUp(KeyCode.Mouse0) && directionMult == -1)
            {
                LetGo();
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1) && directionMult == 1)
            {
                LetGo();
            }

            orbitAcceleration += Time.deltaTime * accelerationMult;

            orbitAcceleration = Mathf.Clamp(orbitAcceleration, minAcceleration, maxAcceleration);
        }
    }

    private void LetGo()
    {
        inOrbit.gameObject.GetComponent<Grabable>().letGo = true;

        inOrbit = null;

        canGrab = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Grabable>() != null && col.gameObject.GetComponent<Grabable>().isGrabable && canGrab)
        {
            grabable.Add(col.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Grabable>() != null && col.gameObject.GetComponent<Grabable>().isGrabable && canGrab && !grabable.Contains(col.gameObject))
        {
            grabable.Add(col.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (grabable.Contains(col.gameObject))
        {
            col.gameObject.GetComponent<Grabable>().beingSelected = false;

            grabable.Remove(col.gameObject);
        }
    }
}
