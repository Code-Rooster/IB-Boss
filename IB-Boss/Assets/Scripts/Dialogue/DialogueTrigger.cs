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
            triggerDialogue();
        }
    }

    public void triggerDialogue()
    {
        if (!dM.startedDialogue)
        {
            dM.startDialogue(dialogue, tFX);
        }
        else if (dM.startedDialogue && !dM.isTyping)
        {
            dM.displayNextSentence();
        }
        else if (dM.startedDialogue && dM.isTyping)
        {
            dM.skipAhead();
        }
    }
}
