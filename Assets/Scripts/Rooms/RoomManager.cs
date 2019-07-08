using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> allRooms = new List<GameObject>();

    public void LoadRooms(List<GameObject> rooms)
    {
        foreach (GameObject room in rooms)
        {
            room.SetActive(true);
        }

        foreach (GameObject room in allRooms)
        {
            if (room != rooms[0] && room != rooms[1])
            {
                room.SetActive(false);
            }
        }
    }
}
