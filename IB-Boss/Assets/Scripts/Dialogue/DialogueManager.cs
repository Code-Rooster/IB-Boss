using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private TextEffects[] textFX;

    VertexAnim[] vertexAnim = new VertexAnim[1024];

    private Queue<string> sentences;

    public TMPro.TMP_Text dialogueText;

    public string currentSentence;

    public bool isTyping;
    public bool startedDialogue = false;
    private bool hasTextChanged;

    public Color32 keyColor;
    public Color32 redColor;
    public Color32 characterColor;
    public Color32 mechanicColor;

    public int sentenceCount;
    public int calledCount;

    List<int> jitterWords = new List<int>();

    public float AngleMultiplier = 1.0f;
    public float SpeedMultiplier = 1.0f;
    public float CurveScale = 1.0f;
    public float typeSpeed = 0.02f;
    public float jitterFactor;

    private struct VertexAnim
    {
        public float angleRange;
        public float angle;
        public float speed;
    }

    void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
    }

    void Start()
    {
        sentences = new Queue<string>();
    }


    void ON_TEXT_CHANGED(Object obj)
    {
        if (obj == dialogueText)
            hasTextChanged = true;
    }

    public void startDialogue(Dialogue dialogue, TextEffects[] tFX)
    {
        sentences.Clear();

        startedDialogue = true;
        sentenceCount = 0;

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        textFX = tFX;

        dialogueText.maxVisibleCharacters = 0;

        displayNextSentence();
    }

    public void displayNextSentence()
    {
        if (sentences.Count == 0)
        {
            endDialogue();
            return;
        }

        StopAllCoroutines();

        currentSentence = sentences.Dequeue();

        vertexAnim = null;
        vertexAnim = new VertexAnim[1024];

        for (int i = 0; i < 1024; i++)
        {
            vertexAnim[i].angleRange = Random.Range(10f, 25f);
            vertexAnim[i].speed = Random.Range(1f, 3f);
        }

        jitterWords.Clear();

        StartCoroutine("typeSentence");
    }

    IEnumerator typeSentence()
    {
        isTyping = true;

        print("Words in this sentence: " + currentSentence.Split(' ').Length);

        dialogueText.text = "";
        dialogueText.text = currentSentence;

        dialogueText.maxVisibleCharacters = 0;

        if (textFX[sentenceCount].jitterAll == true && textFX[sentenceCount].jitterIndexes.Length == 0)
        {
            for (int x = 0; x < currentSentence.Split(' ').Length; x++)
            {
                print("Adding a Jitterword");

                jitterWords.Add(x);
            }
        }

        for (int i = 0; i < currentSentence.ToCharArray().Length; i++)
        {
            dialogueText.maxVisibleCharacters += 1;

            if (textFX[sentenceCount].jitterIndexes.Length != 0 || textFX[sentenceCount].jitterAll)
            {
                StartCoroutine(textMods());
            }

            yield return new WaitForSeconds(typeSpeed);
        }
        isTyping = false;

        sentenceCount += 1;
    }

    public void skipAhead()
    {
        StopCoroutine("typeSentence");
        isTyping = false;

        dialogueText.text = "";
        dialogueText.text = currentSentence;

        dialogueText.maxVisibleCharacters = 0;
        
        dialogueText.maxVisibleCharacters = currentSentence.ToCharArray().Length;

        if (textFX[sentenceCount].jitterIndexes.Length != 0)
        {
            StartCoroutine(textMods());
        }
        else if (textFX[sentenceCount].jitterAll == true)
        {
            for (int x = 0; x < currentSentence.Split(' ').Length; x++)
            {
                jitterWords.Add(x);
            }
            StartCoroutine(textMods());
        }
        sentenceCount += 1;
    }

    public void endDialogue()
    {
        dialogueText.maxVisibleCharacters = 0;
        dialogueText.text = "";

        StopAllCoroutines();

        startedDialogue = false;
    }

    IEnumerator textMods()
    {
        calledCount++;
        //print("Called " + calledCount + " times");

        List<TMP_WordInfo> wInfo;

        wInfo = new List<TMP_WordInfo>();

        int[] wordsToJitter;

        wordsToJitter = null;

        if (jitterWords != null)
        {
            wordsToJitter = jitterWords.ToArray();
        }
        else
        {
            wordsToJitter = textFX[sentenceCount].jitterIndexes;
        }

        //print("WordsToJitter length: " + wordsToJitter.Length);

        while (true)
        {
            dialogueText.ForceMeshUpdate(true);

            wInfo.Clear();

            foreach (int wordIndex in wordsToJitter)
            {
                wInfo.Add(dialogueText.textInfo.wordInfo[wordIndex]);
            }
            //print("wInfo length: " + wInfo.Count);

            Matrix4x4 matrix;

            int loopCount = 0;
            hasTextChanged = true;

            List<TMP_MeshInfo[]> cachedMeshInfo;
            cachedMeshInfo = new List<TMP_MeshInfo[]>();
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

                        TMP_CharacterInfo charInfo = dialogueText.textInfo.characterInfo[charIndex];

                        if (!charInfo.isVisible)
                            continue;

                        VertexAnim vertAnim = vertexAnim[charIndex];

                        int materialIndex = dialogueText.textInfo.characterInfo[charIndex].materialReferenceIndex;

                        int vertexIndex = dialogueText.textInfo.characterInfo[charIndex].vertexIndex;

                        Vector3[] sourceVertices = cMI[materialIndex].vertices;

                        Vector2 charMidBasline = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;

                        Vector3 offset = charMidBasline;

                        Vector3[] destinationVertices = dialogueText.textInfo.meshInfo[materialIndex].vertices;

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
            for (int i = 0; i < dialogueText.textInfo.meshInfo.Length; i++)
            {
                dialogueText.textInfo.meshInfo[i].mesh.vertices = dialogueText.textInfo.meshInfo[i].vertices;
                dialogueText.UpdateGeometry(dialogueText.textInfo.meshInfo[i].mesh, i);
            }

            loopCount += 1;

            yield return new WaitForSeconds(0.1f);
            
        }
    }
}
