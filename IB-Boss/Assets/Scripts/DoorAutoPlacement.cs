using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DoorAutoPlacement : MonoBehaviour
{

    public string parentName = "Doors";
    [HideInInspector]
    public bool alreadyPlaced = false;

    void Awake()
    {
        if (Application.isEditor && !alreadyPlaced)
        {
            transform.parent = GameObject.Find(parentName).transform;
            alreadyPlaced = true;
        }
    }
}
