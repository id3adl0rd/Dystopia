using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public delegate void ExperienceChangeHandler(int amount);

    public event ExperienceChangeHandler OnExperienceChange;

    private void Awake()
    {
        instance = this;
    }

    public void AddExperience(int amount)
    {
        OnExperienceChange?.Invoke(amount);
    }
}