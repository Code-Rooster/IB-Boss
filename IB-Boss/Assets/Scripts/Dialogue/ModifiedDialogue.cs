using System.Collections;
using System.Collections.Generic;

public class ModifiedDialogue
{
    public string sentence;

    public List<int> jitterIndices = new List<int>();
    public List<int> waveIndices = new List<int>();

    //0 = Red
    //1 = Mechanic
    //2 = Character
    //3 = Key
    public List<List<int>> colorIndices = new List<List<int>>();
}
