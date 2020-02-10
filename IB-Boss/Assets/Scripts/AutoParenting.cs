using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[System.Serializable]
public class AutoParenting : MonoBehaviour
{
    private bool alreadyPlaced = false;

    public bool on = false;

    public enum ParentName
    {
        BackWalls,
        FrontWalls,
        LeftWalls,
        RightWalls,
        Doors
    };

    public ParentName parentName;

    private void Awake()
    {
        if (Application.isEditor && !alreadyPlaced && on)
        {
            transform.parent = GameObject.Find(parentName.ToString()).transform;
            alreadyPlaced = true;
        }
    }
}