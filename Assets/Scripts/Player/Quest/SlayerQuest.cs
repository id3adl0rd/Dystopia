using System;

public class SlayerQuest : Quest
{
    private void Awake()
    {
        QuestName = "Slayer";
        Description = "Kill them all";
        ExperienceReward = 100;
        
        Goals.Add(new KillGoal(0, "Kill 5 niggers", false, 0, 5));
        Goals.ForEach(g => g.Init());
    }
}