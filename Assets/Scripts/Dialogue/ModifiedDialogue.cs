using System.Collections;
using System.Collections.Generic;

public class ModifiedDialogue
{
    public string sentence;

    public List<int> jitterIndices = new List<int>();
    public List<int> waveIndices = new List<int>();

    //0 = Key
    //1 = Red
    //2 = Character
    //3 = Mechanic
    public List<int[]> colorIndices = new List<int[]>();
}
