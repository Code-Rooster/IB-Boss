using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject CurrentRoom;

    void Start()
    {
        Screen.fullScreen = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Tab"))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
    }
}
