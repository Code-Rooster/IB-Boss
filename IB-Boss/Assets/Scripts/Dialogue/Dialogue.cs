using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Dialogue
{
    //public string[] sentences;

    public int triggerIndex;

    public string triggerName;

    public enum EndCondtion {NextDialogue, TriggerDialogue, Nothing};

    public EndCondtion endCondition;
}