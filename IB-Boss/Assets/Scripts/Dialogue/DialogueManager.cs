using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Dialogue dialogue;
    private DialogueTrigger dT;
    private ConditionDetection cD;
    private ModifiedDialogue mD;

    public DialogueBox dB;

    VertexAnim[] vertexAnim = new VertexAnim[1024];

    private Queue<string> sentences;

    public TMPro.TMP_Text dialogueText;
    public TMPro.TMP_Text nameText;

    public string currentSentence;

    public bool isTyping;
    public bool startedDialogue = false;
    private bool hasTextChanged;

    public Color32 keyColor;
    public Color32 redColor;
    public Color32 characterColor;
    public Color32 mechanicColor;

    public int sentenceCount;

    public float AngleMultiplier = 1.0f;
    public float SpeedMultiplier = 1.0f;
    public float CurveScale = 1.0f;
    public float typeSpeed = 0.02f;
    public float jitterFactor;

    public Animator dialogueAnim;

    private struct VertexAnim
    {
        public float angleRange;
        public float angle;
        public float speed;
    }

    void OnEnable()
    {
        //Add the ON_TEXT_CHANGED event to TMP's event manager so that it can keep track of text changes
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
    }

    void OnDisable()
    {
        //Remove the ON_TEXT_CHANGED event
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
    }

    void Start()
    {
        sentences = new Queue<string>();

        dB = GameObject.FindGameObjectWithTag("DialogueBox").GetComponent<DialogueBox>();
        cD = this.gameObject.GetComponent<ConditionDetection>();
    }


    void ON_TEXT_CHANGED(Object obj)
    {
        //If the text has just changed notify the rest of the script
        if (obj == dialogueText)
            hasTextChanged = true;
    }

    public void StartDialogue(Dialogue d, string name, DialogueTrigger DT)
    {
        dialogue = d;
        dT = DT;

        nameText.text = name;

        //Clear the sentence queue
        sentences.Clear();

        startedDialogue = true;
        sentenceCount = -1;

        foreach (string sentence in dialogue.sentences)
        {
            //Queue up every sentence
            sentences.Enqueue(sentence);
        }

        //Make all of the characters invisible
        dialogueText.maxVisibleCharacters = 0;

        DisplayNextSentence();

        if (!dB.isOpen)
        {
            //Open the Dialogue Box
            dialogueAnim.Play("OpenDialogue");
        }
    }

    public void DisplayNextSentence()
    {
        sentenceCount += 1;

        if (sentences.Count == 0)
        {
            //If there's no more sentences just return out of the function after ending the dialogue
            EndDialogue();
            return;
        }

        StopAllCoroutines();

        mD = cD.FindMods(sentences.Dequeue());

        //Set the current sentence to the first sentence in the queue
        currentSentence = mD.sentence;

        //Pre determine the vertexAnim variables for the text jitter effect
        vertexAnim = null;
        vertexAnim = new VertexAnim[1024];

        for (int i = 0; i < 1024; i++)
        {
            vertexAnim[i].angleRange = Random.Range(10f, 25f);
            vertexAnim[i].speed = Random.Range(1f, 3f);
        }

        StartCoroutine("TypeSentence");
    }

    IEnumerator TypeSentence()
    {
        yield return new WaitUntil(() => dB.isOpen);

        isTyping = true;
        dialogueText.text = "";
        dialogueText.text = currentSentence;

        //Make all of the characters invisible
        dialogueText.maxVisibleCharacters = 0;

        if (mD.jitterIndices.Count > 0)
        {
            StartCoroutine(TextJitter(mD.jitterIndices.ToArray()));
        }

        //Iterate through every character in the sentence
        for (int i = 0; i < currentSentence.ToCharArray().Length; i++)
        {
            StopCoroutine(TextJitter(mD.jitterIndices.ToArray()));

            //Make the current character visible
            dialogueText.maxVisibleCharacters += 1;

            //if (textFX[sentenceCount].wordColors.keyWords.Length != 0)
            //{
                //SetTextColor(keyColor, textFX[sentenceCount].wordColors.keyWords);
            //}
            //if (textFX[sentenceCount].wordColors.characterWords.Length != 0)
            //{
                //SetTextColor(characterColor, textFX[sentenceCount].wordColors.characterWords);
            //}
            //if (textFX[sentenceCount].wordColors.redWords.Length != 0)
            //{
                //SetTextColor(redColor, textFX[sentenceCount].wordColors.redWords);
            //}
            //if (textFX[sentenceCount].wordColors.mechanicWords.Length != 0)
            //{
                //SetTextColor(mechanicColor, textFX[sentenceCount].wordColors.mechanicWords);
            //}

            yield return new WaitForSeconds(typeSpeed);
        }
        isTyping = false;
    }

    public void SkipAhead()
    {
        //Stop typing
        StopAllCoroutines();
        isTyping = false;

        dialogueText.text = "";
        dialogueText.text = currentSentence;

        //Make all of the characters invisible
        dialogueText.maxVisibleCharacters = 0;

        //Make all of the characters visible
        dialogueText.maxVisibleCharacters = currentSentence.ToCharArray().Length;

        if (mD.jitterIndices.Count > 0)
        {
            StartCoroutine(TextJitter(mD.jitterIndices.ToArray()));
        }

        //if (textFX[sentenceCount].wordColors.keyWords.Length != 0)
        //{
        //SetTextColor(keyColor, textFX[sentenceCount].wordColors.keyWords);
        //}
        //if (textFX[sentenceCount].wordColors.characterWords.Length != 0)
        //{
        //SetTextColor(characterColor, textFX[sentenceCount].wordColors.characterWords);
        //}
        //if (textFX[sentenceCount].wordColors.redWords.Length != 0)
        //{
        //SetTextColor(redColor, textFX[sentenceCount].wordColors.redWords);
        //}
        //if (textFX[sentenceCount].wordColors.mechanicWords.Length != 0)
        //{
        //SetTextColor(mechanicColor, textFX[sentenceCount].wordColors.mechanicWords);
        //}
    }

    public void EndDialogue()
    {
        //Make all of the characters invisible
        dialogueText.maxVisibleCharacters = 0;
        dialogueText.text = "";

        StopAllCoroutines();

        startedDialogue = false;

        switch (dialogue.endCondition)
        {
            case Dialogue.EndCondtion.NextDialogue:
                dialogueAnim.Play("CloseDialogue");
                dT.dialogueIndex++;
                break;
            case Dialogue.EndCondtion.TriggerDialogue:
                dT.dialogueIndex++;
                dialogue.dT.dialogueIndex = dialogue.triggerIndex;
                dialogue.dT.TriggerDialogue();
                break;
            case Dialogue.EndCondtion.Nothing:
                dialogueAnim.Play("CloseDialogue");
                break;
        }
    }

    IEnumerator TextJitter(int[] jitterIndices)
    {
        List<TMP_WordInfo> wInfo;

        wInfo = new List<TMP_WordInfo>();

        while (true)
        {
            //if (textFX[sentenceCount].wordColors.keyWords.Length != 0)
            //{
            //SetTextColor(keyColor, textFX[sentenceCount].wordColors.keyWords);
            //}
            //if (textFX[sentenceCount].wordColors.characterWords.Length != 0)
            //{
            //SetTextColor(characterColor, textFX[sentenceCount].wordColors.characterWords);
            //}
            //if (textFX[sentenceCount].wordColors.redWords.Length != 0)
            //{
            //SetTextColor(redColor, textFX[sentenceCount].wordColors.redWords);
            //}
            //if (textFX[sentenceCount].wordColors.mechanicWords.Length != 0)
            //{
            //SetTextColor(mechanicColor, textFX[sentenceCount].wordColors.mechanicWords);
            //}

            //else
            {
                    dialogueText.ForceMeshUpdate(true);
            }

            wInfo.Clear();

            foreach (int wordIndex in jitterIndices)
            {
                //Add each word's wordInfo
                wInfo.Add(dialogueText.textInfo.wordInfo[wordIndex]);
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

                        TMP_CharacterInfo charInfo = dialogueText.textInfo.characterInfo[charIndex];

                        if (!charInfo.isVisible)
                        {
                            continue;
                        }

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

    public void SetTextColor(Color32 color, int[] wordIndexes)
    {
        dialogueText.ForceMeshUpdate(true);

        for (int j = 0; j < wordIndexes.Length; j++)
        {
            TMP_WordInfo info = dialogueText.textInfo.wordInfo[wordIndexes[j]];
            for (int i = 0; i < info.characterCount; ++i)
            {
                int charIndex = info.firstCharacterIndex + i;
                int meshIndex = dialogueText.textInfo.characterInfo[charIndex].materialReferenceIndex;
                int vertexIndex = dialogueText.textInfo.characterInfo[charIndex].vertexIndex;

                Color32[] vertexColors = dialogueText.textInfo.meshInfo[meshIndex].colors32;
                vertexColors[vertexIndex + 0] = color;
                vertexColors[vertexIndex + 1] = color;
                vertexColors[vertexIndex + 2] = color;
                vertexColors[vertexIndex + 3] = color;
            }
            dialogueText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }
    }
}
