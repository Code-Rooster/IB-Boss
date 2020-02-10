using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public PlayerMovement pM;
    public KeyManager kM;
    public DialogueTrigger dT;
    public YesNoEvents yNE;

    public Transform enterPoint;
    public Transform exitPoint;
    public Transform point;

    private BoxCollider2D entranceCol;
    private BoxCollider2D exitCol;

    private GameObject player;

    public bool backLocked = false;
    public bool frontLocked = false;

    public bool expectUnlock = false;

    public bool canInteract;

    public enum RequiredKey { Bronze, Silver, Gold, CantUnlock }

    public RequiredKey requiredKey;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        pM = player.GetComponent<PlayerMovement>();

        entranceCol = this.transform.Find("Entrance").GetComponent<BoxCollider2D>();
        exitCol = this.transform.Find("Exit").GetComponent<BoxCollider2D>();

        dT = gameObject.GetComponent<DialogueTrigger>();
    }

    private void Update()
    {
        if (point != null)
        {
            if (player.transform.position != point.position)
            {
                player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

                pM.canMove = false;

                player.transform.position = Vector3.Lerp(player.transform.position, point.position, 0.1f);
            }

            if (Vector3.Distance(player.transform.position, point.position) < 0.1)
            {
                point = null;

                pM.canMove = true;
            }
        }

        if (backLocked)
        {
            exitCol.isTrigger = false;
        }
        else if (!backLocked && !exitCol.isTrigger)
        {
            exitCol.isTrigger = true;
        }
        if (frontLocked)
        {
            entranceCol.isTrigger = false;
        }
        else if (!frontLocked && !entranceCol.isTrigger)
        {
            entranceCol.isTrigger = true;
        }

        if (expectUnlock)
        {
            if (yNE.responses[0] != 0)
            {
                if (yNE.responses[0] == 1)
                {
                    if (requiredKey == RequiredKey.Bronze)
                    {
                        kM.bronzeCount--;

                        Unlock();
                    } else if (requiredKey == RequiredKey.Silver)
                    {
                        kM.silverCount--;

                        Unlock();
                    } else if (requiredKey == RequiredKey.Gold)
                    {
                        kM.goldCount--;

                        Unlock();
                    }
                }

                if (Input.GetButtonDown("Submit") && canInteract)
                {
                    dT.TriggerDialogue();

                    expectUnlock = false;
                }
            }
        }
    }

    public void Unlock()
    {
        backLocked = false;
        frontLocked = false;

        entranceCol.isTrigger = true;
        exitCol.isTrigger = true;
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
