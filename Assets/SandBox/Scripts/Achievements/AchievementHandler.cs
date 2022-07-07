using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementHandler : MonoBehaviour
{
    private AchievementsManager achievementsManager;
    private AchievementHINT[] achievementHints;

    public enum MobType
    {
        Nibbler, Redeer, Possessed
    }

    public MobType EnemyType;

    void Start()
    {
        achievementsManager = FindObjectOfType<AchievementsManager>();
        achievementHints = Resources.FindObjectsOfTypeAll<AchievementHINT>();
        foreach (var achievementHint in achievementHints)
        {
            achievementHint.FakeStart();
        }
    }

    public void DiedHandler()
    {
        achievementsManager.IncreaseKilledEnemiesTemp(EnemyType.ToString());
        foreach (var achievementHint in achievementHints)
        {
            achievementHint.FakeStart();
        }
    }
}
