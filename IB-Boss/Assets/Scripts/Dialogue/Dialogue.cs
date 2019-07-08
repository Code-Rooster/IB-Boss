using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Dialogue
{
    public DialogueTrigger dT = null;

    public string[] sentences;

    public int triggerIndex;

    public enum EndCondtion {NextDialogue, TriggerDialogue, Nothing};

    public EndCondtion endCondition;
}