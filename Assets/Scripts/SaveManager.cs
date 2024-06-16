using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [System.Serializable]
    public class UserData
    {
        public int money;
        public int level;
        public int exp;
        public string quest;
        public uint questCount;
        public bool isFinished;
        public string className;
    }

    public static void Save()
    {
        UserData ud = new UserData();
        ud.money = MoneyController.instance.GetMoney();
        ud.level = LevelController.instance._level;
        ud.exp = LevelController.instance._exp;
        if (QuestController.instance.GetQuest() != null)
        {
            ud.quest = QuestController.instance.GetQuest().GetUID();
        }
        else
        {
            ud.quest = "";
        }
        ud.questCount = QuestController.instance.questCount;
        ud.isFinished = QuestController.instance.isFinished;
        ud.className = ClassController.instance.GetClassID();
        string res = JsonUtility.ToJson(ud);
        
        string filePath = Application.persistentDataPath + "/character.json";

        if (File.Exists(filePath) != true)
        {
            FileStream file = File.Create(filePath);
            try
            {
                File.WriteAllText(filePath, res);
            }
            finally
            {
                file.Close();
            }
            
            File.WriteAllText(filePath, res);
        }
        else
        {
            File.WriteAllText(filePath, res);
        }
        
        Debug.Log(Application.persistentDataPath);
    }

    public static void Load()
    {
        string filePath = Application.persistentDataPath + "/character.json";

        string data = System.IO.File.ReadAllText(filePath);
        UserData d = JsonUtility.FromJson<UserData>(data);
        
        Debug.Log(d);
        StaticData.userData = d;
        StaticData.loading = true;
    }
}
