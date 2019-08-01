using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionDetection : MonoBehaviour
{
    string[] modifiers = { "<j>", "<w>", "<R>", "<M>", "<C>", "<K>" };

    public ModifiedDialogue FindMods(string sentence)
    {
        ModifiedDialogue mD = new ModifiedDialogue();

        string finishedSentence = sentence;

        string[] words = sentence.Split(' ');

        int firstIndex = -1;

        List<char> lettersBetween = new List<char>();

        for (int i = 0; i < 4; i++)
        {
            mD.colorIndices.Add(new List<int>());
        }

        for (int x = 0; x < modifiers.Length; x++)
        {
            for (int y = 0; y < words.Length; y++)
            {
                if (words[y].Contains(modifiers[x]))
                {
                    if (words[y].IndexOf(modifiers[x]) != words[y].LastIndexOf(modifiers[x]))
                    {
                        AddWord(mD, modifiers[x], y);

                        words[y].Replace(modifiers[x], "");
                    }
                    else
                    {
                        if (firstIndex == -1)
                        {
                            firstIndex = y;
                        }
                        else
                        {
                            for (int z = firstIndex; z <= y; z++)
                            {
                                AddWord(mD, modifiers[x], z);
                            }

                            words[firstIndex].Replace(modifiers[x], "");
                            words[y].Replace(modifiers[x], "");

                            firstIndex = -1;
                        }
                    }

                    finishedSentence = finishedSentence.Replace(modifiers[x], "");
                }
            }
        }

        mD.sentence = finishedSentence;

        return mD;
    }

    private void AddWord(ModifiedDialogue mD, string modifier, int index)
    {
        if (modifier == modifiers[0])
        {
            mD.jitterIndices.Add(index);
        }
        else if (modifier == modifiers[1])
        {
            mD.waveIndices.Add(index);
        }
        else if (modifier == modifiers[2])
        {
            mD.colorIndices[0].Add(index);
        }
        else if (modifier == modifiers[3])
        {
            mD.colorIndices[1].Add(index);
        }
        else if (modifier == modifiers[4])
        {
            mD.colorIndices[2].Add(index);
        }
        else if (modifier == modifiers[5])
        {
            mD.colorIndices[3].Add(index);
        }
    }
}
