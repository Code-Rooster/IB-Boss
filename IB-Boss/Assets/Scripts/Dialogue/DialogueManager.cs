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
    //private QuestDetection qD;
    public ModifiedDialogue mD;
    private Effects fX;
    public PlayerMovement pM;

    public DialogueBox dB;

    public GameObject yesNoBox;

    public Queue<string> sentences;

    public TMP_Text dialogueText;
    public TMP_Text nameText;

    public TMP_Text yesText;
    public TMP_Text noText;

    public string currentSentence;
    public string dialogueName;

    public bool isTyping;
    public bool tdCheck;
    public bool startedDialogue = false;
    public bool inCutscene;
    public bool endingDialogue;

    public int sentenceCount;
    public int sentencesLeft;
    private int crashProt;

    public float typeSpeed = 0.02f;
    public float jitterSpeed = 1.0f;

    public Animator dialogueAnim;

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            pM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        }

        sentences = new Queue<string>();

        dB = GameObject.FindGameObjectWithTag("DialogueBox").GetComponent<DialogueBox>();

        dialogueText = dB.transform.Find("Dialogue").GetComponent<TMP_Text>();
        nameText = dB.transform.Find("Name").GetComponent<TMP_Text>();
        dialogueAnim = dB.gameObject.GetComponent<Animator>();

        yesNoBox = GameObject.FindGameObjectWithTag("YesNoBox");

        yesText = yesNoBox.transform.Find("YesButton").transform.Find("YesText").GetComponent<TMP_Text>();
        noText = yesNoBox.transform.Find("NoButton").transform.Find("NoText").GetComponent<TMP_Text>();

        cD = this.gameObject.GetComponent<ConditionDetection>();
        fX = this.gameObject.GetComponent<Effects>();

        yesNoBox.SetActive(false);
    }

    public void StartDialogue(Dialogue d, string[] dialogue, string name, DialogueTrigger DT)
    {
        dT = DT;
        D = d;

        if (pM != null)
        {
            pM.canMove = false;
            pM.rb.velocity = Vector2.zero;
        }

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

        if (sentencesLeft <= 0)
        {
            EndDialogue();
            return;
        }

        sentenceCount++;

        StopAllCoroutines();

        currentSentence = sentences.Dequeue();

        mD = cD.FindMods(currentSentence);

        currentSentence = mD.sentence;

        if (mD.yesNoQuestion)
        {
            yesNoBox.SetActive(true);

            yesText.text = mD.yNFirstTerm;
            noText.text = mD.yNSecondTerm;
        }
        else
        {
            yesNoBox.SetActive(false);
        }

        fX.type = true;

        StartCoroutine(fX.ApplyEffects(currentSentence, mD.colorIndices, mD.waveIndices.ToArray(), mD.jitterIndices.ToArray()));
    }

    public void SkipAhead()
    {
        fX.type = false;

        dialogueText.text = "";
        dialogueText.text = currentSentence;
    }

    public void EndDialogue()
    {
        crashProt++;

        if (crashProt < 100)
        {
            dialogueText.text = "";

            StopAllCoroutines();

            switch (D.endCondition)
            {
                case Dialogue.EndCondtion.NextDialogue:

                    dialogueAnim.Play("CloseDialogue");
                    if (D.triggerIndex == -1)
                    {
                        dT.dialogueIndex++;
                    }
                    else
                    {
                        dT.dialogueIndex = D.triggerIndex;
                    }

                    if (pM != null)
                    {
                        pM.canMove = true;
                    }

                    StartCoroutine(WaitForBoxToClose());

                    if (pM != null)
                    {
                        pM.canMove = true;
                    }

                    break;
                case Dialogue.EndCondtion.TriggerDialogue:

                    startedDialogue = false;

                    DialogueTrigger trigger = GameObject.Find(D.triggerName).GetComponent<DialogueTrigger>();

                    sentenceCount = 0;

                    dialogueName = trigger.dialogueName;

                    trigger.dialogueIndex = D.triggerIndex;
                    trigger.TriggerDialogue();

                    break;
                case Dialogue.EndCondtion.Nothing:
                    if (mD.yesNoQuestion)
                    {
                        DialogueTrigger[] allDTs = FindObjectsOfType<DialogueTrigger>();

                        DialogueTrigger yNTrigger = null;

                        for (int i = 0; i < allDTs.Length; i++)
                        {
                            if (i > 100)
                            {
                                break;
                            }
                            if (allDTs[i].dialogueName == mD.yNResponseName)
                            {
                                yNTrigger = allDTs[i];

                                break;
                            }
                        }

                        if (yNTrigger != null && mD.yNFirstResponseTriggerName != "STOP")
                        {
                            startedDialogue = false;

                            sentenceCount = 0;

                            dialogueName = yNTrigger.dialogueName;

                            yNTrigger.dialogueIndex = mD.yNResponseIndex;

                            yNTrigger.TriggerDialogue();

                            break;
                        }
                        else
                        {
                            dialogueAnim.Play("CloseDialogue");
                            if (pM != null)
                            {
                                pM.canMove = true;
                            }

                            StartCoroutine(WaitForBoxToClose());

                            break;
                        }
                    }
                    else
                    {
                        dialogueAnim.Play("CloseDialogue");

                        if (pM != null)
                        {
                            pM.canMove = true;
                        }

                        StartCoroutine(WaitForBoxToClose());
                    }
                    break;
                default:
                    dialogueAnim.Play("CloseDialogue");

                    if (pM != null)
                    {
                        pM.canMove = true;
                    }

                    StartCoroutine(WaitForBoxToClose());

                    break;
            }
        }
    }

    private IEnumerator WaitForBoxToClose()
    {
        endingDialogue = true;

        while (dB.isOpen)
        {
            yield return null;
        }

        inCutscene = false;

        startedDialogue = false;

        if (pM != null)
        {
            pM.canMove = true;
        }

        endingDialogue = false;
    }
}
