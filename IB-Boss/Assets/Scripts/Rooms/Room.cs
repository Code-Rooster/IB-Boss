using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Manager manager;
    private SmartCam sC;

    public Transform[] camLims = {null, null};

    public List<GameObject> adjacentRooms = new List<GameObject>();

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
        sC = GameObject.Find("CameraBox").GetComponent<SmartCam>();
    }

    private void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Room")
        {
            if (!adjacentRooms.Contains(col.gameObject))
            {
                adjacentRooms.Add(col.gameObject);
            }
        }
    }

    public void LoadRooms()
    {
        foreach (GameObject room in GameObject.FindGameObjectsWithTag("Room"))
        {
            if (!adjacentRooms.Contains(room) && room != this.gameObject)
            {
                foreach (Transform roomChild in room.transform)
                {
                    roomChild.gameObject.SetActive(false);
                }
            }
            else if (adjacentRooms.Contains(room))
            {
                foreach (Transform roomChild in room.transform)
                {
                    roomChild.gameObject.SetActive(true);
                }
            }
        }

        manager.CurrentRoom = this.gameObject;
        sC.room = this;
    }

    public void UnloadRoom()
    {

    }
}
