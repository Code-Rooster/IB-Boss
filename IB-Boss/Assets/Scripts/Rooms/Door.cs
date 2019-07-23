using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public PlayerMovement pM;

    public Transform enterPoint;
    public Transform exitPoint;
    public Transform point;

    private BoxCollider2D entranceCol;
    private BoxCollider2D exitCol;

    private GameObject player;

    public bool backLocked = false;
    public bool frontLocked = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        pM = player.GetComponent<PlayerMovement>();

        entranceCol = this.transform.Find("Entrance").GetComponent<BoxCollider2D>();
        exitCol = this.transform.Find("Exit").GetComponent<BoxCollider2D>();

        if (backLocked)
        {
            exitCol.isTrigger = false;
        }
        if (frontLocked)
        {
            entranceCol.isTrigger = false;
        }
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
    }

    public void Unlock()
    {
        print("... You unlocked the door!");

        entranceCol.isTrigger = true;
        exitCol.isTrigger = true;
    }
}
