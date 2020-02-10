using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    private Door door;

    private void Start()
    {
        door = this.transform.parent.GetComponent<Door>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (door.canInteract && door.backLocked)
            {
                if (door.requiredKey == Door.RequiredKey.Bronze)
                {
                    if (door.kM.bronzeCount >= 1)
                    {
                        //print("Should be one");

                        door.dT.dialogueIndex = 1;

                        door.dT.TriggerDialogue();

                        door.expectUnlock = true;
                    }
                    else
                    {
                        door.dT.dialogueIndex = 0;

                        door.dT.TriggerDialogue();
                    }
                }
                else if (door.requiredKey == Door.RequiredKey.Silver)
                {
                    if (door.kM.silverCount >= 1)
                    {
                        door.dT.dialogueIndex = 2;

                        door.expectUnlock = true;

                        door.dT.TriggerDialogue();
                    }
                    else
                    {
                        door.dT.dialogueIndex = 0;

                        door.dT.TriggerDialogue();
                    }
                }
                else if (door.requiredKey == Door.RequiredKey.Gold)
                {
                    if (door.kM.goldCount >= 1)
                    {
                        door.dT.dialogueIndex = 3;

                        door.expectUnlock = true;

                        door.dT.TriggerDialogue();
                    }
                    else
                    {
                        door.dT.dialogueIndex = 0;

                        door.dT.TriggerDialogue();
                    }
                }
                else
                {
                    door.dT.dialogueIndex = 0;

                    door.dT.TriggerDialogue();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && door.point == null)
        {
            if (door.frontLocked)
            {
                door.Unlock();
            }

            door.point = door.exitPoint;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Player")
        {
            print("... It's locked.");
        }
    }
}
