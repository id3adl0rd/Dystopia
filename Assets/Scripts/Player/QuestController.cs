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
        
        Debug.Log(StaticData.isQuestFinished);
        Debug.Log(StaticData.elimination);
        if (StaticData.isQuestFinished == true)
        {
            isFinished = true;
        }
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
        GameObject.Find("QuestText").GetComponent<TMP_Text>().text = _quest.GetDescription();
    }
}