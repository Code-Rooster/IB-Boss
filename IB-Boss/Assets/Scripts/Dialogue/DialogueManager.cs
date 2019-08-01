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
    private QuestDetection qD;
    public ModifiedDialogue mD;
    private Effects fX;

    public DialogueBox dB;

    private Queue<string> sentences;

    public TMP_Text dialogueText;
    public TMP_Text nameText;

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

        qD = GameObject.FindGameObjectWithTag("QM").GetComponent<QuestDetection>();

        cD = this.gameObject.GetComponent<ConditionDetection>();
        fX = this.gameObject.GetComponent<Effects>();
    }

    public void StartDialogue(Dialogue d, string[] dialogue, string name, DialogueTrigger DT)
    {
        dT = DT;
        D = d;

        nameText.text = name;

        sentences.Clear();

        startedDialogue = true;
        sentenceCount = -1;

        foreach (string sentence in dialogue)
        {
            if (sentence != null)
            {
                sentences.Enqueue(sentence);
            }
        }

        DisplayNextSentence();

        if (!dB.isOpen)
        {
            dialogueAnim.Play("OpenDialogue");
        }
    }

    public void DisplayNextSentence()
    {
        sentenceCount += 1;

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        StopAllCoroutines();

        currentSentence = qD.FindQuests(sentences.Dequeue());

        mD = cD.FindMods(currentSentence);

        currentSentence = mD.sentence;

        fX.type = true;

        StartCoroutine(fX.ApplyEffects(currentSentence, mD.colorIndices, mD.waveIndices.ToArray(), mD.jitterIndices.ToArray()));
    }

    public void SkipAhead()
    {
        //Stop typing
        fX.type = false;

        dialogueText.text = "";
        dialogueText.text = currentSentence;
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
