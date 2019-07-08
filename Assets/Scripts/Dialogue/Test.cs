using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Test : MonoBehaviour
{
    public TMP_Text dialogueText;
    public Color32 newColor;

    // Start is called before the first frame update
    void Start()
    {
        SetTextColor(newColor);
    }

    public void SetTextColor(Color32 color)
    {
        dialogueText.ForceMeshUpdate(true);

        for (int j = 0; j < dialogueText.textInfo.wordCount - 1; j++)
        {
            TMP_WordInfo info = dialogueText.textInfo.wordInfo[j];
            for (int i = 0; i < info.characterCount; ++i)
            {
                int charIndex = info.firstCharacterIndex + i;
                int meshIndex = dialogueText.textInfo.characterInfo[charIndex].materialReferenceIndex;
                int vertexIndex = dialogueText.textInfo.characterInfo[charIndex].vertexIndex;

                Color32[] vertexColors = dialogueText.textInfo.meshInfo[meshIndex].colors32;
                vertexColors[vertexIndex + 0] = newColor;
                vertexColors[vertexIndex + 1] = newColor;
                vertexColors[vertexIndex + 2] = newColor;
                vertexColors[vertexIndex + 3] = newColor;
            }

            dialogueText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }
    }
}
