using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class YesNoResponse : MonoBehaviour
{
    public DialogueManager dM;
    public YesNoEvents yNE;

    private void Start()
    {
        dM = GameObject.FindGameObjectWithTag("DM").GetComponent<DialogueManager>();
        yNE = GameObject.FindGameObjectWithTag("DM").GetComponent<YesNoEvents>();
    }

    public void AnswerYes()
    {
        print("Answered Yes " + dM.mD.yNIndex);

        dM.mD.yNResponseName = dM.mD.yNFirstResponseTriggerName;
        dM.mD.yNResponseIndex = dM.mD.yNFirstResponseTriggerIndex;
        yNE.responses[dM.mD.yNIndex] = 1;
    }

    public void AnswerNo()
    {
        dM.mD.yNResponseName = dM.mD.yNSecondResponseTriggerName;
        dM.mD.yNResponseIndex = dM.mD.yNSecondResponseTriggerIndex;
        yNE.responses[dM.mD.yNIndex] = -1;
    }
}
