using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private DialogueManager dM;

    private Animator anim;

    public bool canPause = true;
    public static bool isPaused = false;

    void Start()
    {
        dM = GameObject.FindGameObjectWithTag("DM").GetComponent<DialogueManager>();

        Screen.fullScreen = false;

        anim = this.gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (dM.startedDialogue)
        {
            canPause = false;
        }
        else
        {
            canPause = true;
        }

        if (Input.GetButtonDown("Cancel") && canPause)
        {
            if (!isPaused)
            {
                anim.Play("PauseGame");
            }
            else
            {
                anim.Play("UnpauseGame");
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;

        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        isPaused = false;

        Time.timeScale = 1;
    }
}
