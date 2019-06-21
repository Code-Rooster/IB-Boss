using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telekinesis : MonoBehaviour
{
    private Vector3 mousePos;

    public Color32 highlightColor;
    public Color32 activeColor;

    public GameObject hoveringOver;

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.z = -1;

        transform.position = mousePos;

        if (hoveringOver != null)
        {
            hoveringOver.transform.position = mousePos;

            if (Input.GetMouseButtonUp(0))
            {
                hoveringOver.transform.Find("Light").GetComponent<Light>().color = highlightColor;

                hoveringOver = null;
            }
        }
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
