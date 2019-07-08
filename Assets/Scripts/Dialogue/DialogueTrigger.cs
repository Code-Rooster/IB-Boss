using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public TextEffects[] tFX;

    private DialogueManager dM;

    private void Start()
    {
        dM = GameObject.FindGameObjectWithTag("DM").GetComponent<DialogueManager>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        if (!dM.startedDialogue)
        {
            dM.StartDialogue(dialogue, tFX);
        }
        else if (dM.startedDialogue && !dM.isTyping && dM.dB.isOpen)
        {
            dM.DisplayNextSentence();
        }
        else if (dM.startedDialogue && dM.isTyping && dM.dB.isOpen)
        {
            dM.SkipAhead();
        }
    }
}
