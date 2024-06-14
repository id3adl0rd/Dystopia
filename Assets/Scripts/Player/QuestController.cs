using System;
using TMPro;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    [SerializeField] private Quest _quest;
    public uint questCount;
    public bool isFinished;
    
    public static QuestController instance;
    
    private void Awake()
    {
        instance = this;
        questCount = 0;
        isFinished = false;
        
        isFinished = StaticData.isQuestFinished;
        questCount = StaticData.elimination;
        _quest = StaticData.quest;
    }

    public void ClearQuest()
    {
        StaticData.isQuestFinished = false;
        StaticData.elimination = 0;
        StaticData.quest = null;
        isFinished = false;
        questCount = 0;
        _quest = null;
    }

    public Quest GetQuest()
    {
        return _quest;
    }

    public void SetQuest(Quest quest)
    {
        _quest = quest;
        questCount = 0;
    }
    
    public void OnKillObj(string objname)
    {
        if (_quest == null) return;
        if (isFinished == true) return;
        
        if (_quest.isKillQuest == true)
        {
            Debug.Log(objname);
            
            if (_quest.GetObject() == objname)
            {
                questCount++;
                _ = _quest.IsFinished();
            }
        }
    }

    public void SetQuestData()
    {
        if (_quest == null)
            GameObject.Find("QuestText").GetComponent<TMP_Text>().text = "Нет задания";
        else
            GameObject.Find("QuestText").GetComponent<TMP_Text>().text = _quest.GetDescription();
    }
}