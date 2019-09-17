using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetTheEntertainer : MonoBehaviour
{
    private Cutscenes cutscenes;
    private DialogueManager dM;

    public GameObject player;
    public GameObject entertainer;

    private DialogueTrigger entertainerDT;

    public float speed1;

    public Vector3 pos1 = new Vector3(0, 0, 0);

    public bool startedCutscene = false;

    private void Start()
    {
        cutscenes = GameObject.Find("CutsceneManager").GetComponent<Cutscenes>();
        dM = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
    }

    private IEnumerator StartCutscene()
    {
        startedCutscene = true;

        player.GetComponent<PlayerMovement>().canMove = false;

        entertainerDT = entertainer.GetComponent<DialogueTrigger>();

        StartCoroutine(cutscenes.MoveToPos(player, pos1, speed1));

        entertainerDT.dialogueIndex = 0;
        entertainerDT.TriggerDialogue();

        yield return new WaitUntil(() => dM.startedDialogue == false);

        player.GetComponent<PlayerMovement>().canMove = true;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player" && !startedCutscene)
        {
            StartCoroutine(StartCutscene());
        }
    }
}
