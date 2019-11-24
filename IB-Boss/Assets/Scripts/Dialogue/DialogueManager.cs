using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Dialogue D;
    public DialogueTrigger dT;
    private ConditionDetection cD;
    private QuestDetection qD;
    public ModifiedDialogue mD;
    private Effects fX;
    public PlayerMovement pM;

    public DialogueBox dB;

    public Queue<string> sentences;

    public TMP_Text dialogueText;
    public TMP_Text nameText;

    public string currentSentence;
    public string dialogueName;

    public bool isTyping;
    public bool tdCheck;
    public bool startedDialogue = false;

    public int sentenceCount;
    public int sentencesLeft;

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

        pM.canMove = false;
        pM.rb.velocity = Vector2.zero;

        startedDialogue = true;
        tdCheck = false;
        sentenceCount = 0;

        nameText.text = name;

        dialogueName = name;

        sentences.Clear();

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
        sentencesLeft = sentences.Count;

        if (sentences.Count <= 0)
        {
            EndDialogue();
            return;
        }

        sentenceCount++;

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

                pM.canMove = true;

                break;
            case Dialogue.EndCondtion.TriggerDialogue:
                DialogueTrigger trigger = GameObject.Find(D.triggerName).GetComponent<DialogueTrigger>();

                sentenceCount = 0;

                dialogueName = trigger.dialogueName;

                trigger.dialogueIndex = D.triggerIndex;
                trigger.TriggerDialogue();
                break;
            case Dialogue.EndCondtion.Nothing:
                dialogueAnim.Play("CloseDialogue");

                pM.canMove = true;

                break;
            default:
                dialogueAnim.Play("CloseDialogue");

                pM.canMove = true;

                break;
        }
    }
}
