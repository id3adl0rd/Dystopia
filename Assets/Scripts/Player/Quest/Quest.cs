using UnityEngine;

[CreateAssetMenu]
public class Quest : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private string uid;
    public bool isKillQuest;
    [SerializeField] private uint count;
    [SerializeField] private string killObject;

    public string GetName()
    {
        return name;
    }
    
    public string GetDescription()
    {
        return string.Format("{0} - {1} из {2}", name, QuestController.instance.questCount, count);
    }

    public string GetUID()
    {
        return uid;
    }

    public string GetObject()
    {
        return killObject;
    }
    
    public uint GetCount()
    {
        return count;
    }

    public bool IsFinished()
    {
        if (QuestController.instance.questCount >= count)
        {
            //NotifyController.instance.AddToQueue("Квест завершен!", 0f);
            QuestController.instance.isFinished = true;
            return true;
        }

        return false;
    }
}