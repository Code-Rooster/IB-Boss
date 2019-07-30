using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

[System.Serializable]

public class ColorEffect : MonoBehaviour
{
    private DialogueManager dM;

    public Color32[] colors;

    public bool isColoring;

    private void Start()
    {
        dM = this.gameObject.GetComponent<DialogueManager>();
    }

    public void ColorText()
    {
        int[][] wordList = new int[][] {
            new int[1]
        };

        wordList[0][0] = 0;

        dM.dialogueText.ForceMeshUpdate(true);

        isColoring = true;

        for (int x = 0; x < wordList.Length; x++)
        {
            if (wordList[x] != null)
            {
                for (int j = 0; j < wordList[x].Length; j++)
                {
                    print("Word list [" + x.ToString() + "] length : " + wordList[x].Length.ToString());
                    print("Word list [" + x.ToString() + "][" + j.ToString() + "] value : " + wordList[x][j].ToString());

                    TMP_WordInfo info = dM.dialogueText.textInfo.wordInfo[wordList[x][j]];
                    for (int i = 0; i < info.characterCount; ++i)
                    {
                        int charIndex = info.firstCharacterIndex + i;
                        int meshIndex = dM.dialogueText.textInfo.characterInfo[charIndex].materialReferenceIndex;
                        int vertexIndex = dM.dialogueText.textInfo.characterInfo[charIndex].vertexIndex;

                        Color32[] vertexColors = dM.dialogueText.textInfo.meshInfo[meshIndex].colors32;
                        vertexColors[vertexIndex + 0] = colors[x];
                        vertexColors[vertexIndex + 1] = colors[x];
                        vertexColors[vertexIndex + 2] = colors[x];
                        vertexColors[vertexIndex + 3] = colors[x];
                    }
                    dM.dialogueText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
                }
            }
        }
    }
}
