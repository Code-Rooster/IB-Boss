using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class Effects : MonoBehaviour
{
    private DialogueManager dM;

    public float AngleMultiplier = 1.0f;
    public float CurveScale = 1.0f;
    public float waveFrequency;
    private float[] initialYVal = new float[500];
    private float[] initialXVal = new float[500];

    private bool hasTextChanged;

    public bool type;

    //0 = Red
    //1 = Mechanic
    //2 = Character
    //3 = Key
    public Color32[] colors;

    VertexAnim[] vertexAnim = new VertexAnim[1024];

    void ON_TEXT_CHANGED(Object obj)
    {
        //If the text has just changed notify the rest of the script
        if (obj == dM.dialogueText)
        {
            hasTextChanged = true;
        }
    }

    private struct VertexAnim
    {
        public float angleRange;
        public float angle;
        public float speed;
    }

    private void Start()
    {
        dM = this.gameObject.GetComponent<DialogueManager>();
    }

    public IEnumerator ApplyEffects(string sentence, List<List<int>> colorIndices, int[] waveIndices, int[] jitterIndices)
    {
        int loopCount = 0;
        int lastLoopCount = 0;
        int currentCharTyping = 0;

        float lastTime = Time.time;

        List<TMP_WordInfo> jitterInfo;
        jitterInfo = new List<TMP_WordInfo>();

        if (jitterIndices.Length > 0)
        {
            vertexAnim = null;
            vertexAnim = new VertexAnim[1024];

            for (int i = 0; i < 1024; i++)
            {
                vertexAnim[i].angleRange = Random.Range(10f, 25f);
                vertexAnim[i].speed = Random.Range(1f, 3f);
            }
        }

        List<TMP_WordInfo> waveInfo;
        waveInfo = new List<TMP_WordInfo>();

        if (type)
        {
            dM.dialogueText.text = "";

            yield return new WaitUntil(() => dM.dB.isOpen);

            dM.isTyping = true;

            dM.dialogueText.text = sentence;
            dM.dialogueText.maxVisibleCharacters = sentence.ToCharArray().Length;

            dM.dialogueText.ForceMeshUpdate(true);

            ColorAllCharacters(0);
        }
        else
        {
            dM.dialogueText.text = sentence;
        }

        while (true)
        {
            if (type && currentCharTyping < sentence.ToCharArray().Length)
            {
                int waitCycles = 2;

                if (currentCharTyping > 0)
                {
                    char lastCharTyped = sentence.ToCharArray()[currentCharTyping - 1];

                    if (lastCharTyped != '.' || lastCharTyped != ',' || lastCharTyped != '?' || lastCharTyped != '!')
                    {
                        waitCycles = 2;

                        if (sentence.ToCharArray()[currentCharTyping] == ' ')
                        {
                            currentCharTyping++;
                        }
                    }

                    else
                    {
                        waitCycles = 5;
                    }
                }

                if (lastLoopCount + waitCycles <= loopCount)
                {
                    TMP_TextInfo typeInfo = dM.dialogueText.textInfo;

                    Color32 typeColor = typeInfo.textComponent.color;

                    typeColor.a = 255;

                    for (int x = 0; x < colorIndices.Count; x++)
                    {
                        TMP_TextInfo colorDialogueInfo = dM.dialogueText.textInfo;

                        if (colorIndices[x].Count != 0)
                        {
                            for (int y = 0; y < colorIndices[x].Count; y++)
                            {
                                TMP_WordInfo ColorWordInfo = colorDialogueInfo.wordInfo[colorIndices[x][y]];

                                for (int z = 0; z < ColorWordInfo.characterCount; z++)
                                {
                                    if (currentCharTyping == ColorWordInfo.firstCharacterIndex + z)
                                    {
                                        typeColor = colors[x];
                                    }
                                }
                            }
                        }
                    }

                    int typeMeshIndex = dM.dialogueText.textInfo.characterInfo[currentCharTyping].materialReferenceIndex;
                    int typeVertexIndex = dM.dialogueText.textInfo.characterInfo[currentCharTyping].vertexIndex;

                    if (dM.dialogueText.text.ToCharArray()[currentCharTyping] == sentence.ToCharArray()[currentCharTyping])
                    {

                        Color32[] typeVertexColors = dM.dialogueText.textInfo.meshInfo[typeMeshIndex].colors32;

                        typeVertexColors[typeVertexIndex + 0] = typeColor;
                        typeVertexColors[typeVertexIndex + 1] = typeColor;
                        typeVertexColors[typeVertexIndex + 2] = typeColor;
                        typeVertexColors[typeVertexIndex + 3] = typeColor;

                        dM.dialogueText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

                        if (currentCharTyping == 3)
                        {
                            print(sentence.ToCharArray()[currentCharTyping] + ": " + typeColor);
                        }
                    }
                    else
                    {
                        print("error");
                    }

                    if (currentCharTyping < sentence.ToCharArray().Length)
                    {
                        currentCharTyping++;
                    }

                    lastLoopCount = loopCount;
                }
            }

            if (currentCharTyping >= sentence.ToCharArray().Length)
            {
                dM.isTyping = false;

                type = false;

                currentCharTyping = 0;
            }

            if (!type)
            {
                for (int x = 0; x < colorIndices.Count; x++)
                {
                    TMP_TextInfo colorDialogueInfo = dM.dialogueText.textInfo;

                    if (colorIndices[x].Count != 0)
                    {
                        for (int y = 0; y < colorIndices[x].Count; y++)
                        {
                            TMP_WordInfo ColorWordInfo = colorDialogueInfo.wordInfo[colorIndices[x][y]];

                            for (int z = 0; z < ColorWordInfo.characterCount; z++)
                            {
                                int colorTypeMeshIndex = dM.dialogueText.textInfo.characterInfo[ColorWordInfo.firstCharacterIndex + z].materialReferenceIndex;
                                int colorTypeVertexIndex = dM.dialogueText.textInfo.characterInfo[ColorWordInfo.firstCharacterIndex + z].vertexIndex;

                                Color32[] colorTypeVertexColors = dM.dialogueText.textInfo.meshInfo[colorTypeMeshIndex].colors32;

                                colorTypeVertexColors[colorTypeVertexIndex + 0] = colors[x];
                                colorTypeVertexColors[colorTypeVertexIndex + 1] = colors[x];
                                colorTypeVertexColors[colorTypeVertexIndex + 2] = colors[x];
                                colorTypeVertexColors[colorTypeVertexIndex + 3] = colors[x];

                                dM.dialogueText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
                            }
                        }
                    }
                }

                if (dM.isTyping && !type)
                {
                    dM.isTyping = false;

                    ColorAllCharacters(255);
                }
            }

            if (jitterIndices.Length > 0 && dM.dialogueText.textInfo.wordCount >= jitterIndices[0])
            {

                jitterInfo.Clear();

                foreach (int jitterIndex in jitterIndices)
                {
                    jitterInfo.Add(dM.dialogueText.textInfo.wordInfo[jitterIndex]);
                }

                Matrix4x4 jitterMatrix;

                hasTextChanged = true;

                List<TMP_MeshInfo[]> cachedJitterMeshInfo;
                cachedJitterMeshInfo = new List<TMP_MeshInfo[]>();

                foreach (TMP_WordInfo wordInfo in jitterInfo)
                {
                    cachedJitterMeshInfo.Add(wordInfo.textComponent.textInfo.CopyMeshInfoVertexData());
                }

                if (hasTextChanged)
                {
                    foreach (TMP_WordInfo wordInfo in jitterInfo)
                    {
                        cachedJitterMeshInfo.Add(wordInfo.textComponent.textInfo.CopyMeshInfoVertexData());
                    }

                    hasTextChanged = false;
                }

                foreach (TMP_WordInfo info in jitterInfo)
                {
                    foreach (TMP_MeshInfo[] cMI in cachedJitterMeshInfo)
                    {
                        int jitterCharacterCount = info.characterCount;

                        if (jitterCharacterCount == 0)
                        {
                            yield return new WaitForSeconds(0.25f);
                            continue;
                        }

                        for (int i = 0; i < jitterCharacterCount; i++)
                        {
                            int jitterCharIndex = info.firstCharacterIndex + i;

                            TMP_CharacterInfo jitterCharInfo = dM.dialogueText.textInfo.characterInfo[jitterCharIndex];

                            if (!jitterCharInfo.isVisible)
                            {
                                continue;
                            }

                            VertexAnim vertAnim = vertexAnim[jitterCharIndex];

                            int jitterMaterialIndex = dM.dialogueText.textInfo.characterInfo[jitterCharIndex].materialReferenceIndex;

                            int jitterVertexIndex = dM.dialogueText.textInfo.characterInfo[jitterCharIndex].vertexIndex;

                            Vector3[] jitterSourceVertices = cMI[jitterMaterialIndex].vertices;

                            Vector2 jitterCharMidBasline = (jitterSourceVertices[jitterVertexIndex + 0] + jitterSourceVertices[jitterVertexIndex + 2]) / 2;

                            Vector3 jitterOffset = jitterCharMidBasline;

                            Vector3[] jitterDestinationVertices = dM.dialogueText.textInfo.meshInfo[jitterMaterialIndex].vertices;

                            if (loopCount == 0)
                            {
                                SetInitialCoordinates(jitterCharIndex, jitterOffset);
                            }

                            jitterDestinationVertices[jitterVertexIndex + 0] = jitterSourceVertices[jitterVertexIndex + 0] - jitterOffset;
                            jitterDestinationVertices[jitterVertexIndex + 1] = jitterSourceVertices[jitterVertexIndex + 1] - jitterOffset;
                            jitterDestinationVertices[jitterVertexIndex + 2] = jitterSourceVertices[jitterVertexIndex + 2] - jitterOffset;
                            jitterDestinationVertices[jitterVertexIndex + 3] = jitterSourceVertices[jitterVertexIndex + 3] - jitterOffset;

                            vertAnim.angle = Mathf.SmoothStep(-vertAnim.angleRange, vertAnim.angleRange, Mathf.PingPong(loopCount / 25f * vertAnim.speed, 1f));
                            Vector3 jitterEffectOffset = new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f), 0);

                            jitterMatrix = Matrix4x4.TRS(jitterEffectOffset * CurveScale, Quaternion.Euler(0, 0, Random.Range(-5f, 5f) * AngleMultiplier), Vector3.one);

                            jitterDestinationVertices[jitterVertexIndex + 0] = jitterMatrix.MultiplyPoint3x4(jitterDestinationVertices[jitterVertexIndex + 0]);
                            jitterDestinationVertices[jitterVertexIndex + 1] = jitterMatrix.MultiplyPoint3x4(jitterDestinationVertices[jitterVertexIndex + 1]);
                            jitterDestinationVertices[jitterVertexIndex + 2] = jitterMatrix.MultiplyPoint3x4(jitterDestinationVertices[jitterVertexIndex + 2]);
                            jitterDestinationVertices[jitterVertexIndex + 3] = jitterMatrix.MultiplyPoint3x4(jitterDestinationVertices[jitterVertexIndex + 3]);

                            jitterDestinationVertices[jitterVertexIndex + 0].y += initialYVal[jitterCharIndex];
                            jitterDestinationVertices[jitterVertexIndex + 1].y += initialYVal[jitterCharIndex];
                            jitterDestinationVertices[jitterVertexIndex + 2].y += initialYVal[jitterCharIndex];
                            jitterDestinationVertices[jitterVertexIndex + 3].y += initialYVal[jitterCharIndex];

                            jitterDestinationVertices[jitterVertexIndex + 0].x += initialXVal[jitterCharIndex];
                            jitterDestinationVertices[jitterVertexIndex + 1].x += initialXVal[jitterCharIndex];
                            jitterDestinationVertices[jitterVertexIndex + 2].x += initialXVal[jitterCharIndex];
                            jitterDestinationVertices[jitterVertexIndex + 3].x += initialXVal[jitterCharIndex];

                            jitterDestinationVertices[jitterVertexIndex + 0] += jitterEffectOffset;
                            jitterDestinationVertices[jitterVertexIndex + 1] += jitterEffectOffset;
                            jitterDestinationVertices[jitterVertexIndex + 2] += jitterEffectOffset;
                            jitterDestinationVertices[jitterVertexIndex + 3] += jitterEffectOffset;

                            vertexAnim[i] = vertAnim;
                        }
                    }
                }
            }

            if (waveIndices.Length > 0 && dM.dialogueText.textInfo.wordCount >= waveIndices[0])
            {
                waveInfo.Clear();

                foreach (int waveIndex in waveIndices)
                {
                    //Add each word's wordInfo
                    waveInfo.Add(dM.dialogueText.textInfo.wordInfo[waveIndex]);
                }

                Matrix4x4 waveMatrix;

                hasTextChanged = true;

                List<TMP_MeshInfo[]> cachedWaveMeshInfo;
                cachedWaveMeshInfo = new List<TMP_MeshInfo[]>();
                cachedWaveMeshInfo.Clear();

                foreach (TMP_WordInfo wordInfo in waveInfo)
                {
                    cachedWaveMeshInfo.Add(wordInfo.textComponent.textInfo.CopyMeshInfoVertexData());
                }

                if (hasTextChanged)
                {
                    foreach (TMP_WordInfo wordInfo in waveInfo)
                    {
                        cachedWaveMeshInfo.Add(wordInfo.textComponent.textInfo.CopyMeshInfoVertexData());
                    }

                    hasTextChanged = false;
                }

                foreach (TMP_WordInfo info in waveInfo)
                {
                    int waveCharacterCount = info.characterCount;

                    foreach (TMP_MeshInfo[] cMI in cachedWaveMeshInfo)
                    {

                        if (waveCharacterCount == 0)
                        {
                            yield return new WaitForSeconds(0.25f);
                            continue;
                        }

                        for (int i = 0; i < waveCharacterCount; i++)
                        {

                            int waveCharIndex = info.firstCharacterIndex + i;

                            TMP_CharacterInfo waveCharInfo = dM.dialogueText.textInfo.characterInfo[waveCharIndex];

                            if (!waveCharInfo.isVisible)
                            {
                                continue;
                            }

                            int waveMaterialIndex = dM.dialogueText.textInfo.characterInfo[waveCharIndex].materialReferenceIndex;

                            int waveVertexIndex = dM.dialogueText.textInfo.characterInfo[waveCharIndex].vertexIndex;

                            Vector3[] waveSourceVertices = cMI[waveMaterialIndex].vertices;

                            Vector2 waveCharMidBasline = (waveSourceVertices[waveVertexIndex + 0] + waveSourceVertices[waveVertexIndex + 2]) / 2;

                            Vector3 waveOffset = waveCharMidBasline;

                            Vector3[] waveDestinationVertices = dM.dialogueText.textInfo.meshInfo[waveMaterialIndex].vertices;

                            if (loopCount == 0)
                            {
                                SetInitialCoordinates(waveCharIndex, waveOffset);
                            }

                            waveDestinationVertices[waveVertexIndex + 0] = waveSourceVertices[waveVertexIndex + 0] - waveOffset;
                            waveDestinationVertices[waveVertexIndex + 1] = waveSourceVertices[waveVertexIndex + 1] - waveOffset;
                            waveDestinationVertices[waveVertexIndex + 2] = waveSourceVertices[waveVertexIndex + 2] - waveOffset;
                            waveDestinationVertices[waveVertexIndex + 3] = waveSourceVertices[waveVertexIndex + 3] - waveOffset;

                            Vector3 waveEffectOffset = new Vector3(0, Mathf.Cos((waveCharIndex * waveFrequency) - Time.time * 6) * 2, 0);

                            waveMatrix = Matrix4x4.TRS(waveEffectOffset, Quaternion.Euler(0, 0, 0), Vector3.one);

                            waveDestinationVertices[waveVertexIndex + 0] = waveMatrix.MultiplyPoint3x4(waveDestinationVertices[waveVertexIndex + 0]);
                            waveDestinationVertices[waveVertexIndex + 1] = waveMatrix.MultiplyPoint3x4(waveDestinationVertices[waveVertexIndex + 1]);
                            waveDestinationVertices[waveVertexIndex + 2] = waveMatrix.MultiplyPoint3x4(waveDestinationVertices[waveVertexIndex + 2]);
                            waveDestinationVertices[waveVertexIndex + 3] = waveMatrix.MultiplyPoint3x4(waveDestinationVertices[waveVertexIndex + 3]);

                            waveDestinationVertices[waveVertexIndex + 0].y += waveEffectOffset.y;
                            waveDestinationVertices[waveVertexIndex + 1].y += waveEffectOffset.y;
                            waveDestinationVertices[waveVertexIndex + 2].y += waveEffectOffset.y;
                            waveDestinationVertices[waveVertexIndex + 3].y += waveEffectOffset.y;
                            

                            waveDestinationVertices[waveVertexIndex + 0].y += initialYVal[waveCharIndex];
                            waveDestinationVertices[waveVertexIndex + 1].y += initialYVal[waveCharIndex];
                            waveDestinationVertices[waveVertexIndex + 2].y += initialYVal[waveCharIndex];
                            waveDestinationVertices[waveVertexIndex + 3].y += initialYVal[waveCharIndex];

                            waveDestinationVertices[waveVertexIndex + 0].x += waveOffset.x;
                            waveDestinationVertices[waveVertexIndex + 1].x += waveOffset.x;
                            waveDestinationVertices[waveVertexIndex + 2].x += waveOffset.x;
                            waveDestinationVertices[waveVertexIndex + 3].x += waveOffset.x;
                        }
                    }
                }
            }

            for (int i = 0; i < dM.dialogueText.textInfo.meshInfo.Length; i++)
            {
                dM.dialogueText.textInfo.meshInfo[i].mesh.vertices = dM.dialogueText.textInfo.meshInfo[i].vertices;
                dM.dialogueText.UpdateGeometry(dM.dialogueText.textInfo.meshInfo[i].mesh, i);
            }

            loopCount += 1;

            lastTime = Time.time;

            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    private void ColorAllCharacters(byte alpha)
    {
        TMP_TextInfo typeInfo = dM.dialogueText.textInfo;

        Color32 typeColor = typeInfo.textComponent.color;
        typeColor.a = alpha;

        for (int y = 0; y < typeInfo.characterCount; y++)
        {
            int typeCharIndex = y;
            int typeMeshIndex = dM.dialogueText.textInfo.characterInfo[typeCharIndex].materialReferenceIndex;
            int typeVertexIndex = dM.dialogueText.textInfo.characterInfo[typeCharIndex].vertexIndex;

            Color32[] typeVertexColors = dM.dialogueText.textInfo.meshInfo[typeMeshIndex].colors32;

            typeVertexColors[typeVertexIndex + 0] = typeColor;
            typeVertexColors[typeVertexIndex + 1] = typeColor;
            typeVertexColors[typeVertexIndex + 2] = typeColor;
            typeVertexColors[typeVertexIndex + 3] = typeColor;

            dM.dialogueText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

        }
    }

    private void SetInitialCoordinates(int index, Vector3 initialOffset)
    {
        initialYVal[index] = initialOffset.y;
        initialXVal[index] = initialOffset.x;
    }
}
