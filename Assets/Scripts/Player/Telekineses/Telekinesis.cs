using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telekinesis : MonoBehaviour
{
    private Vector3 mousePos;
    private Vector3 lastPos;

    public float lerpSpeed;
    public float attackSpeed;

    public Color32 highlightColor;
    public Color32 activeColor;

    public GameObject hoveringOver;
    
    public Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        lastPos = transform.position;
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.z = -1;

        transform.position = Vector3.Lerp(transform.position, mousePos, lerpSpeed);

        Vector3 posDiff = transform.position - lastPos;

        //print(posDiff);

        if (hoveringOver != null)
        {
            hoveringOver.transform.position = transform.position;

            if (Input.GetMouseButtonUp(0))
            {
                hoveringOver.transform.Find("Light").GetComponent<Light>().color = highlightColor;

                hoveringOver = null;
            }
        }

        if (Mathf.Abs(posDiff.x) > 1 || Mathf.Abs(posDiff.y) > 1)
        {
            //print("Speed Reached");
        }

        attackSpeed = CalculateSpeed(posDiff);

        //print(attackSpeed);

        lastPos = transform.position;
    }

    private float CalculateSpeed(Vector3 PosDiff)
    {
        float xSqr = Mathf.Pow(PosDiff.x, 2);
        float ySqr = Mathf.Pow(PosDiff.y, 2);
        float result = Mathf.Sqrt(xSqr + ySqr);

        return result;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Moveable")
        {
            col.transform.Find("Light").GetComponent<Light>().color = highlightColor;

            col.transform.Find("Light").gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Moveable")
        {
            if (Input.GetMouseButton(0))
            {
                hoveringOver = col.gameObject;

                col.transform.Find("Light").GetComponent<Light>().color = activeColor;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Moveable")
        {
            col.transform.Find("Light").gameObject.SetActive(false);

            hoveringOver = null;
        }
    }
}
