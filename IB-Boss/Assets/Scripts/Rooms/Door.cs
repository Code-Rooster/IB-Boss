using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Room room;
    public Door linkedDoor;
    public PlayerMovement pM;

    public Transform playerPoint;

    private GameObject player;

    private bool enteredThisDoor;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        pM = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (pM.canEnter == false)
        {
            if (player.transform.position != linkedDoor.playerPoint.position && enteredThisDoor == true)
            {
                player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

                pM.canMove = false;

                player.transform.position = Vector3.Lerp(player.transform.position, linkedDoor.playerPoint.position, 0.1f);
            }

            if (Vector3.Distance(player.transform.position, linkedDoor.playerPoint.position) < 0.1 && enteredThisDoor == true)
            {
                enteredThisDoor = false;

                pM.canMove = true;
                pM.canEnter = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        print("Collided");

        if (pM.canEnter)
        {
            print("Entered: " + this.gameObject.name);

            if (col.tag == "Player")
            {
                room.LoadRooms();

                enteredThisDoor = true;

                pM.canEnter = false;
            }
        }
    }
}
