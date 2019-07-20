using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[SerializeField]
public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager dM;
    public Dialogue[] dialogue;

    public string name;

    public int dialogueIndex;

    private bool canInteract = false;

    private void Start()
    {
        dM = GameObject.FindGameObjectWithTag("DM").GetComponent<DialogueManager>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit") && canInteract)
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        if (!dM.startedDialogue)
        {
            dM.StartDialogue(dialogue[dialogueIndex], name, this);
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            canInteract = false;
        }
    }
}
