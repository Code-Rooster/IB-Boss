using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Dialogue
{
    //public string[] sentences;

    public int triggerIndex = -1;

    public string triggerName;

    public enum EndCondtion {NextDialogue, TriggerDialogue, Nothing};

    public EndCondtion endCondition;
}