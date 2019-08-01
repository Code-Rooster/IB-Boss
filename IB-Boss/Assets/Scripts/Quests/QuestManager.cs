using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public Animator anim;

    public GameObject questBox;

    public TMP_Text questText;

    public Quest[] quests;
    public List<Quest> activeQuests = new List<Quest>();
    public List<Quest> completedQuests = new List<Quest>();

    private void Start()
    {
        anim = questBox.GetComponent<Animator>();
    }

    public void StartQuest(Quest quest)
    {
        activeQuests.Add(quest);

        questText.text = "<u>" + '"' + quest.questName + '"' + "</u>";

        anim.Play("QuestBoxOpen");
    }

    public void EndQuest(Quest quest)
    {
        activeQuests.Remove(quest);
        completedQuests.Add(quest);

        questText.text = "<s><u>" + '"' + quest.questName + '"' + "</u></s>";

        anim.Play("QuestBoxOpen");
    }
}
