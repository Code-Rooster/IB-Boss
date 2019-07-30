using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    private Door door;

    private void Start()
    {
        door = this.transform.parent.GetComponent<Door>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && door.point == null)
        {
            if (door.backLocked)
            {
                door.Unlock();
            }

            door.point = door.enterPoint;
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
