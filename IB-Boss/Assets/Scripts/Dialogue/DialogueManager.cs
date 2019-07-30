using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Dialogue D;
    private DialogueTrigger dT;
    private ConditionDetection cD;
    public ModifiedDialogue mD;
    //private JitterEffect jE;
    //private WaveEffect wE;
    private Effects fX;
    private ColorEffect cE;

    public DialogueBox dB;

    private Queue<string> sentences;

    public TMPro.TMP_Text dialogueText;
    public TMPro.TMP_Text nameText;

    public string currentSentence;

    public bool isTyping;
    public bool startedDialogue = false;

    public int sentenceCount;

    public float typeSpeed = 0.02f;
    public float jitterSpeed = 1.0f;

    public Animator dialogueAnim;

    void Start()
    {
        sentences = new Queue<string>();

        dB = GameObject.FindGameObjectWithTag("DialogueBox").GetComponent<DialogueBox>();
        cD = this.gameObject.GetComponent<ConditionDetection>();
        //jE = this.gameObject.GetComponent<JitterEffect>();
        //wE = this.gameObject.GetComponent<WaveEffect>();
        cE = this.gameObject.GetComponent<ColorEffect>();
        fX = this.gameObject.GetComponent<Effects>();
    }

    public void StartDialogue(Dialogue d, string[] dialogue, string name, DialogueTrigger DT)
    {
        dT = DT;
        D = d;

        nameText.text = name;

        //Clear the sentence queue
        sentences.Clear();

        startedDialogue = true;
        sentenceCount = -1;

        foreach (string sentence in dialogue)
        {
            if (sentence != null)
            {
                //Queue up every sentence
                sentences.Enqueue(sentence);
            }
        }

        //Make all of the characters invisible
        //dialogueText.maxVisibleCharacters = 0;

        DisplayNextSentence();

        if (!dB.isOpen)
        {
            //Open the Dialogue Box
            dialogueAnim.Play("OpenDialogue");
        }
    }

    public void DisplayNextSentence()
    {
        cE.isColoring = false;

        sentenceCount += 1;

        if (sentences.Count == 0)
        {
            //If there's no more sentences just return out of the function after ending the dialogue
            EndDialogue();
            return;
        }

        StopAllCoroutines();

        mD = cD.FindMods(sentences.Dequeue());

        currentSentence = mD.sentence;

        dialogueText.text = "";

        StartCoroutine(fX.ApplyEffects(currentSentence, true, mD.waveIndices.ToArray(), mD.jitterIndices.ToArray()));
    }

    public void SkipAhead()
    {
        //Stop typing
        StopAllCoroutines();
        isTyping = false;

        dialogueText.text = "";
        dialogueText.text = currentSentence;

        //StartCoroutine(fX.ApplyEffects(currentSentence, true, mD.waveIndices.ToArray(), mD.jitterIndices.ToArray()));
    }

    public void EndDialogue()
    {
        dialogueText.text = "";

        StopAllCoroutines();

        startedDialogue = false;

        switch (D.endCondition)
        {
            case Dialogue.EndCondtion.NextDialogue:
                dialogueAnim.Play("CloseDialogue");
                dT.dialogueIndex++;
                break;
            case Dialogue.EndCondtion.TriggerDialogue:
                dT.dialogueIndex++;
                GameObject.Find(D.triggerName).GetComponent<DialogueTrigger>().dialogueIndex = D.triggerIndex;
                GameObject.Find(D.triggerName).GetComponent<DialogueTrigger>().TriggerDialogue();
                break;
            case Dialogue.EndCondtion.Nothing:
                dialogueAnim.Play("CloseDialogue");
                break;
            default:
                dialogueAnim.Play("CloseDialogue");
                break;
        }
    }
}
