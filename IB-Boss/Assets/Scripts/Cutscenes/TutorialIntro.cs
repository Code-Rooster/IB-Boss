using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialIntro : MonoBehaviour
{

    public DialogueTrigger dT;
    private DialogueManager dM;

    bool cutsceneStarted;

    private void Start()
    {
        dM = GameObject.FindGameObjectWithTag("DM").GetComponent<DialogueManager>();
    }

    private void StartCutscene()
    {
        cutsceneStarted = true;

        dM.inCutscene = true;

        dT.TriggerDialogue();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!cutsceneStarted)
        {
            if (col.tag == "Player")
            {
                StartCutscene();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!cutsceneStarted)
        {
            if (col.tag == "Player")
            {
                StartCutscene();
            }
        }
    }
}
