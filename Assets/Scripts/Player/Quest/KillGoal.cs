using System.Collections;
using System.Collections.Generic;
using NPC;
using UnityEngine;

public class KillGoal : Goal
{
    public int EnemyID { get; set; }

    public KillGoal(int eID, string desc, bool completed, int currentAmount, int requiredAmount)
    {
        this.EnemyID = eID;
        this.Description = desc;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
    }

    public override void Init()
    {
        base.Init();
    }

    void EnemyDied(IEnemy enemy)
    {
        if (enemy.ID == this.EnemyID)
        {
            this.CurrentAmount++;
            Evaluate();
        }
    }
}
