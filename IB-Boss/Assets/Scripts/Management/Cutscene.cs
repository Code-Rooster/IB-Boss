using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public DialogueTrigger initialTrigger;

    public int dialogueIndex;

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            StartCutscene(initialTrigger, dialogueIndex);
        }
    }

    private void StartCutscene(DialogueTrigger trigger, int index)
    {
        trigger.dialogueIndex = index;

        trigger.TriggerDialogue();

        this.gameObject.SetActive(false);
    }
}
