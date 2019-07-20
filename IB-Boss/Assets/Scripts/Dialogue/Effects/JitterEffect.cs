using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

[System.Serializable]

public class JitterEffect : MonoBehaviour
{
    private DialogueManager dM;
    private ColorEffect cE;

    public float AngleMultiplier = 1.0f;
    public float SpeedMultiplier = 1.0f;
    public float CurveScale = 1.0f;

    private bool hasTextChanged;

    VertexAnim[] vertexAnim = new VertexAnim[1024];

    private void Start()
    {
        dM = this.gameObject.GetComponent<DialogueManager>();
        cE = this.gameObject.GetComponent<ColorEffect>();
    }

    private struct VertexAnim
    {
        public float angleRange;
        public float angle;
        public float speed;
    }

    void ON_TEXT_CHANGED(Object obj)
    {
        //If the text has just changed notify the rest of the script
        if (obj == dM.dialogueText)
            hasTextChanged = true;
    }

    public IEnumerator TextJitter(int[] jitterIndices)
    {
        List<TMP_WordInfo> wInfo;

        wInfo = new List<TMP_WordInfo>();

        //Pre determine the vertexAnim variables for the text jitter effect
        //THIS MAY CAUSE LAG
        vertexAnim = null;
        vertexAnim = new VertexAnim[1024];

        for (int i = 0; i < 1024; i++)
        {
            vertexAnim[i].angleRange = Random.Range(10f, 25f);
            vertexAnim[i].speed = Random.Range(1f, 3f);
        }
        //Lag over


        while (true)
        {
            if (!cE.isColoring)
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

                        VertexAnim vertAnim = vertexAnim[charIndex];

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

                        vertAnim.angle = Mathf.SmoothStep(-vertAnim.angleRange, vertAnim.angleRange, Mathf.PingPong(loopCount / 25f * vertAnim.speed, 1f));
                        Vector3 jitterOffset = new Vector3(Random.Range(-.25f, .25f), Random.Range(-.25f, .25f), 0);

                        matrix = Matrix4x4.TRS(jitterOffset * CurveScale, Quaternion.Euler(0, 0, Random.Range(-5f, 5f) * AngleMultiplier), Vector3.one);

                        destinationVertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 0]);
                        destinationVertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 1]);
                        destinationVertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 2]);
                        destinationVertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 3]);

                        destinationVertices[vertexIndex + 0] += offset;
                        destinationVertices[vertexIndex + 1] += offset;
                        destinationVertices[vertexIndex + 2] += offset;
                        destinationVertices[vertexIndex + 3] += offset;

                        vertexAnim[i] = vertAnim;
                    }
                }
            }
            for (int i = 0; i < dM.dialogueText.textInfo.meshInfo.Length; i++)
            {
                dM.dialogueText.textInfo.meshInfo[i].mesh.vertices = dM.dialogueText.textInfo.meshInfo[i].vertices;
                dM.dialogueText.UpdateGeometry(dM.dialogueText.textInfo.meshInfo[i].mesh, i);
            }

            loopCount += 1;

            yield return new WaitForSeconds(0.1f);

        }
    }
}
