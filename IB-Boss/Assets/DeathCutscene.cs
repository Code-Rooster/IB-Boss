using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCutscene : MonoBehaviour
{
    public bool started = false;

    private void Update()
    {
        if (!started)
        {
            GameObject.FindGameObjectWithTag("DM").GetComponent<DialogueManager>().inCutscene = true;

            gameObject.GetComponent<DialogueTrigger>().dialogueIndex = Random.Range(0, 3);
            gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();

            started = true;
        }
    }
}
