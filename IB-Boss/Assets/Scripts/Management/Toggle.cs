using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : MonoBehaviour
{
    public GameObject[] toggleObjects;

    public void SwitchState()
    {
        for (int i = 0; i < toggleObjects.Length; i++)
        {
            toggleObjects[i].SetActive(!toggleObjects[i].activeSelf);
        }
    }
}
