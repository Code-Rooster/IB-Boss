using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[SerializeField]
public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager dM;
    private Dialogue[] dialogue = new Dialogue[100];

    public string dialogueName;
    public string[] lines;
    public string[][] dialogues = new string[100][];

    public int dialogueIndex;

    private bool canInteract = false;

    private void Start()
    {
        dM = GameObject.FindGameObjectWithTag("DM").GetComponent<DialogueManager>();

        TextAsset textFile = Resources.Load<TextAsset>("Dialogue/Characters/" + dialogueName);

        lines = textFile.text.Split('\n');

        int dialogueCount = 0;
        int dialogueDepth = 0;

        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogues[i] = new string[20];
        }

        for (int i = 0; i < lines.Length; i++)
        {
            if (!lines[i].Contains("|"))
            {
                dialogues[dialogueCount][dialogueDepth] = lines[i];
                dialogueDepth++;
            }
            else
            {
                dialogueCount++;
                dialogueDepth = 0;
            }
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (canInteract)
            {
                TriggerDialogue();
            }
        }
    }

    public void TriggerDialogue()
    {
        dialogue[dialogueIndex] = new Dialogue();

        if (!dM.startedDialogue && !dM.isTyping)
        {
            string firstDialogue = dialogues[dialogueIndex][0];

            if (firstDialogue.Contains("{ND}"))
            {
                dialogue[dialogueIndex].endCondition = Dialogue.EndCondtion.NextDialogue;

                dialogues[dialogueIndex][0] = firstDialogue.Replace("{ND}", "");
            }
            else if (firstDialogue.Contains("{TD}"))
            {
                dialogue[dialogueIndex].endCondition = Dialogue.EndCondtion.TriggerDialogue;

                for (int i = System.Array.IndexOf(firstDialogue.ToCharArray(), '[') + 1; i < firstDialogue.ToCharArray().Length; i++)
                {
                    if (firstDialogue.ToCharArray()[i] != ']')
                    {
                        dialogue[dialogueIndex].triggerName += firstDialogue.ToCharArray()[i];
                    }
                    else
                    {
                        break;
                    }
                }

                string toParse = null;

                for (int i = System.Array.IndexOf(firstDialogue.ToCharArray(), '(') + 1; i < firstDialogue.ToCharArray().Length; i++)
                {
                    if (firstDialogue.ToCharArray()[i] != ')')
                    {
                        toParse += firstDialogue.ToCharArray()[i];
                    }
                    else
                    {
                        break;
                    }
                }

                int.TryParse(toParse.ToString(), out dialogue[dialogueIndex].triggerIndex);

                dialogues[dialogueIndex][0] = firstDialogue.Replace("{TD}", "");
                dialogues[dialogueIndex][0] = dialogues[dialogueIndex][0].Replace("(" + toParse + ")", "");
                dialogues[dialogueIndex][0] = dialogues[dialogueIndex][0].Replace("[" + dialogue[dialogueIndex].triggerName + "]", "");
            }
            else
            {
                dialogue[dialogueIndex].endCondition = Dialogue.EndCondtion.Nothing;
            }

            dM.StartDialogue(dialogue[dialogueIndex], dialogues[dialogueIndex], dialogueName, this);
        }
        else if (dM.startedDialogue && dM.tdCheck && dM.dB.isOpen)
        {
            dM.DisplayNextSentence();
        }
        else if (dM.startedDialogue && dM.isTyping && dM.dB.isOpen)
        {
            dM.SkipAhead();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            canInteract = false;
        }
    }
}
