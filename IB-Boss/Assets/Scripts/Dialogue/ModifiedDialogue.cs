using System.Collections;
using System.Collections.Generic;

public class ModifiedDialogue
{
    public string sentence;

    public List<int> jitterIndices = new List<int>();
    public List<int> waveIndices = new List<int>();

    public bool yesNoQuestion;
    public bool essayQuestion;

    public string yNFirstTerm;
    public string yNSecondTerm;

    public string yNBool;

    public string yNFirstResponseTriggerName;
    public string yNSecondResponseTriggerName;
    public int yNFirstResponseTriggerIndex;
    public int yNSecondResponseTriggerIndex;

    public string yNResponseName;
    public int yNResponseIndex;

    //0 = Red
    //1 = Mechanic
    //2 = Character
    //3 = Key
    public List<List<int>> colorIndices = new List<List<int>>();
}
