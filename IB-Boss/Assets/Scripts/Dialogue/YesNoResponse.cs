using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YesNoResponse : MonoBehaviour
{
    public DialogueManager dM;
    public ResponseRecorder rR;

    public void AnswerYes()
    {
        dM.mD.yNResponseName = dM.mD.yNFirstResponseTriggerName;
        dM.mD.yNResponseIndex = dM.mD.yNFirstResponseTriggerIndex;
    }

    public void AnswerNo()
    {
        dM.mD.yNResponseName = dM.mD.yNSecondResponseTriggerName;
        dM.mD.yNResponseIndex = dM.mD.yNSecondResponseTriggerIndex;
    }
}
