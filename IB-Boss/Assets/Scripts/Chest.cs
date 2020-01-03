using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chest : MonoBehaviour
{
    public bool empty;
    private bool playerCanInteract;

    public int dialogueIndex;

    public UnityEvent onOpen;

    private void Update()
    {
        if (playerCanInteract && Input.GetButtonDown("Submit"))
        {
            if (!empty)
            {
                gameObject.GetComponent<DialogueTrigger>().dialogueIndex = dialogueIndex;

                Open();
            }

            gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();
        }
    }

    public void Open()
    {
        onOpen.Invoke();

        empty = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            playerCanInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            playerCanInteract = false;
        }
    }
}
