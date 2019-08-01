using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDetection : MonoBehaviour
{
    private QuestManager qM;

    private void Start()
    {
        qM = this.gameObject.GetComponent<QuestManager>();
    }

    public string FindQuests(string sentence)
    {
        string questName = "";
        string finishedSentence = sentence;

        char[] sentenceArray = sentence.ToCharArray();

        if (sentence.Contains("<SQ:"))
        {
            for (int x = System.Array.IndexOf(sentenceArray, ':') + 1; x < sentenceArray.Length; x++)
            {
                if (sentenceArray[x] != '>')
                {
                    questName += sentenceArray[x];
                }
                else
                {
                    break;
                }
            }

            if (questName != "")
            {
                for (int y = 0; y < qM.quests.Length; y++)
                {
                    if (qM.quests[y].questName == questName)
                    {
                        print("Found Quest: " + questName);

                        qM.StartQuest(qM.quests[y]);
                    }
                }
            }

            finishedSentence = sentence.Replace("<SQ:" + questName + ">", "");
        }

        if (sentence.Contains("<EQ:"))
        {
            for (int x = System.Array.IndexOf(sentenceArray, ':') + 1; x < sentenceArray.Length; x++)
            {
                if (sentenceArray[x] != '>')
                {
                    questName += sentenceArray[x];
                }
                else
                {
                    break;
                }
            }

            if (questName != "")
            {
                for (int y = 0; y < qM.quests.Length; y++)
                {
                    if (qM.quests[y].questName == questName)
                    {
                        qM.EndQuest(qM.quests[y]);
                    }
                }
            }

            if (finishedSentence != sentence)
            {
                finishedSentence = finishedSentence.Replace("<EQ:" + questName + ">", "");
            }
            else
            {
                finishedSentence = sentence.Replace("<EQ:" + questName + ">", "");
            }
        }

        return finishedSentence;
    }
}
