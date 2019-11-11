using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : MonoBehaviour
{
    private GameObject outline;

    public bool isGrabable = true;
    public bool beingSelected = false;

    private void Start()
    {
        outline = GameObject.Find("Outline");

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
    }
}
