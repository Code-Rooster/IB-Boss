using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject CurrentRoom;

    public bool canPause = true;
    public bool isPaused = false;

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

        if (Input.GetButtonDown("Cancel") && canPause)
        {
            if (!isPaused)
            {
                isPaused = true;

                Time.timeScale = 0;
            }
            else
            {
                isPaused = false;

                Time.timeScale = 1;
            }
        }
    }
}
