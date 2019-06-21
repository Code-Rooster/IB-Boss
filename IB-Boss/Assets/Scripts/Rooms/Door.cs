using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private RoomManager rM;

    public List<GameObject> roomsToLoad = new List<GameObject>(2);
    

    // Start is called before the first frame update
    void Start()
    {
        rM = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            rM.LoadRooms(roomsToLoad);
        }
    }
}
