using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class WaveEffect : MonoBehaviour
{
    private DialogueManager dM;
    private ColorEffect cE;

    private float waveCount;

    public float AngleMultiplier = 1.0f;
    public float CurveScale = 1.0f;

    public bool isWaving;
    private bool hasTextChanged;

    private void Start()
    {
        dM = this.gameObject.GetComponent<DialogueManager>();
        cE = this.gameObject.GetComponent<ColorEffect>();
    }

    void ON_TEXT_CHANGED(Object obj)
    {
        //If the text has just changed notify the rest of the script
        if (obj == dM.dialogueText)
        {
            hasTextChanged = true;
        }
    }

    public IEnumerator Wave(int[] jitterIndices)
    {
        List<TMP_WordInfo> wInfo;

        wInfo = new List<TMP_WordInfo>();

        isWaving = true;

        while (true)
        {
            if (!cE.isColoring && !hasTextChanged)
            {
                dM.dialogueText.ForceMeshUpdate(true);
            }
            else
            {
                cE.ColorText(dM.mD.colorIndices.ToArray());
            }

            wInfo.Clear();

            foreach (int wordIndex in jitterIndices)
            {
                //Add each word's wordInfo
                wInfo.Add(dM.dialogueText.textInfo.wordInfo[wordIndex]);
            }

            Matrix4x4 matrix;

            int loopCount = 0;
            hasTextChanged = true;

            List<TMP_MeshInfo[]> cachedMeshInfo;
            cachedMeshInfo = new List<TMP_MeshInfo[]>();
            cachedMeshInfo.Clear();

            foreach (TMP_WordInfo wordInfo in wInfo)
            {
                cachedMeshInfo.Add(wordInfo.textComponent.textInfo.CopyMeshInfoVertexData());
            }

            if (hasTextChanged)
            {
                foreach (TMP_WordInfo wordInfo in wInfo)
                {
                    cachedMeshInfo.Add(wordInfo.textComponent.textInfo.CopyMeshInfoVertexData());
                }

                hasTextChanged = false;
            }

            waveCount += Time.fixedUnscaledDeltaTime * 10;

            foreach (TMP_WordInfo info in wInfo)
            {
                foreach (TMP_MeshInfo[] cMI in cachedMeshInfo)
                {
                    int characterCount = info.characterCount;

                    if (characterCount == 0)
                    {
                        yield return new WaitForSeconds(0.25f);
                        continue;
                    }

                    for (int i = 0; i < characterCount; i++)
                    {
                        int charIndex = info.firstCharacterIndex + i;

                        TMP_CharacterInfo charInfo = dM.dialogueText.textInfo.characterInfo[charIndex];

                        if (!charInfo.isVisible)
                        {
                            continue;
                        }

                        int materialIndex = dM.dialogueText.textInfo.characterInfo[charIndex].materialReferenceIndex;

                        int vertexIndex = dM.dialogueText.textInfo.characterInfo[charIndex].vertexIndex;

                        Vector3[] sourceVertices = cMI[materialIndex].vertices;

                        Vector2 charMidBasline = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;

                        Vector3 offset = charMidBasline;

                        Vector3[] destinationVertices = dM.dialogueText.textInfo.meshInfo[materialIndex].vertices;

                        destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] - offset;
                        destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - offset;
                        destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - offset;
                        destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - offset;

                        Vector3 waveOffset = new Vector3(0, Mathf.Sin(charIndex + waveCount) * 5f, 0);

                        matrix = Matrix4x4.TRS(waveOffset, Quaternion.Euler(0, 0, 0), Vector3.one);

                        destinationVertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 0]);
                        destinationVertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 1]);
                        destinationVertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 2]);
                        destinationVertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 3]);

                        destinationVertices[vertexIndex + 0] += offset;
                        destinationVertices[vertexIndex + 1] += offset;
                        destinationVertices[vertexIndex + 2] += offset;
                        destinationVertices[vertexIndex + 3] += offset;
                    }
                }
            }

            for (int i = 0; i < dM.dialogueText.textInfo.meshInfo.Length; i++)
            {
                dM.dialogueText.textInfo.meshInfo[i].mesh.vertices = dM.dialogueText.textInfo.meshInfo[i].vertices;
                dM.dialogueText.UpdateGeometry(dM.dialogueText.textInfo.meshInfo[i].mesh, i);
            }

            yield return new WaitForSeconds(0.2f / 3);

            loopCount++;
        }
    }
}
