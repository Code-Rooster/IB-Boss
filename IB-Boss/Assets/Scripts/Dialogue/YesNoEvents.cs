using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class YesNoEvents : MonoBehaviour
{
    //REMEMBER: TRIGGER EVENTS FROM HERE BY DETECTING CHANGES IN THE INDICES WITH UPDATE() AND THEN CALLING FUNCTIONS

    //[0]: Whether or not to open a door
    //[1]: Whether or not the player's been through the tutorial
    //[2]: Continue or Quit
    public List<int> responses = new List<int>();

    private void Awake()
    {
        while (responses.Count < 100)
        {
            responses.Add(0);
        }
    }
}
