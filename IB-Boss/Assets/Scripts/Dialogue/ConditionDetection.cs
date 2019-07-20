using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ConditionDetection : MonoBehaviour
{
    string[] modifiers = { "~", "#", "%" };

    Regex[] modReges = {new Regex(@"\~.*?\~"), new Regex(@"\#.*?#"), new Regex(@"\%.*?%") };

    public ModifiedDialogue FindMods(string sentence)
    {
        ModifiedDialogue mD = new ModifiedDialogue();

        string finishedSentence = sentence;

        List<int> jitterIndices = new List<int>();
        List<int> keyIndices = new List<int>();
        List<int> redIndices = new List<int>();

        for (int x = 0; x < modifiers.Length; x++)
        {
        List<string> modWords = new List<string>();

            if (sentence.Contains(modifiers[x]))
            {
                modWords.Clear();

                int startingIndex = 0;

                string[] sentenceWords = sentence.Replace(modifiers[x], "").Split(' ');

                finishedSentence = finishedSentence.Replace(modifiers[x], "");

                MatchCollection modMatches = modReges[x].Matches(sentence);

                startingIndex = System.Array.IndexOf(sentence.Split(' '), modMatches[0].Value.Split(' ')[0]);

                for (int i = 0; i < modMatches.Count; i++)
                {
                    foreach (string word in modMatches[i].Value.Split(' '))
                    {
                        modWords.Add(word.Replace(modifiers[x], ""));
                    }
                }

                for (int i = 0; i < modWords.Count; i++)
                {
                    if (modifiers[x] == "~")
                    {
                        jitterIndices.Add(i + startingIndex);
                    }
                    else if (modifiers[x] == "#")
                    {
                        keyIndices.Add(i + startingIndex);
                    }
                    else if (modifiers[x] == "%")
                    {
                        redIndices.Add(i + startingIndex);
                    }
                }
            }
        }

        mD.jitterIndices = jitterIndices;

        mD.colorIndices.Clear();
        if (keyIndices.Count > 0)
        {
            mD.colorIndices.Add(keyIndices.ToArray());
        }
        else
        {
            mD.colorIndices.Add(null);
        }
        mD.sentence = finishedSentence;

        if (redIndices.Count > 0)
        {
            mD.colorIndices.Add(redIndices.ToArray());
        }
        else
        {
            mD.colorIndices.Add(null);
        }

        mD.sentence = finishedSentence;

        return mD;
    }
}
