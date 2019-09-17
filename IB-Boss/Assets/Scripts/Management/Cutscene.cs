using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public DialogueTrigger initialTrigger;

    public string[] instructions;

    public TextAsset cutsceneFile;

    public int dialogueIndex;
    public int currentInstruction = 0;

    private void Start()
    {
        instructions = cutsceneFile.text.Split('\n');
    }

    
}
