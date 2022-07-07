using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsHandlerTEST : MonoBehaviour
{
    private AchievementsManager achievementsManager;

    public enum MobType
    {
        Nibbler, Redeer, Possessed
    }
    public MobType EnemyType;

    void Start()
    {
        achievementsManager = FindObjectOfType<AchievementsManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
//        print("Fucking unity is not working how it supposed to");
        achievementsManager.IncreaseKilledEnemiesTemp(EnemyType.ToString());
    }
}
