using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject CurrentRoom;

    public static LayerMask letGoCollisions;

    void Start()
    {
        Screen.fullScreen = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Tab"))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
