using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementHINT : MonoBehaviour
{
    Text text;
    private AchievementsManager achievementsManager;
    [SerializeField] string patternBASE, patternLEFT, patternALL, ending;

    public enum Achievements
    {
        DemonKiller,
        GhostHunter,
        Exorcist,
        YouShallNotPass,
        RichGuy,
        Bersek
    }

    public Achievements achievement;

    private Dictionary<Achievements, int> AtAllDick;
    private Dictionary<Achievements, int> LeftDick;

    void Awake()
    {

    }
    // Start is called before the first frame update
    public void FakeStart()
    {
        Text text = GetComponent<Text>();
        achievementsManager = FindObjectOfType<AchievementsManager>();
        AtAllDick = new Dictionary<Achievements, int>()
        {
            {Achievements.DemonKiller, achievementsManager.enemiesToGetDemonKiller},
            {Achievements.GhostHunter, achievementsManager.enemiesToGetGhostHunter},
            {Achievements.Exorcist, achievementsManager.enemiesToGetExorcism},
            // Achievements.YouShallNotPass, none
            {Achievements.RichGuy, achievementsManager.spendToGetRichGuy},
            {Achievements.Bersek, achievementsManager.killsWith1HPToGetBersek},
        };
        LeftDick = new Dictionary<Achievements, int>()
        {
            {Achievements.DemonKiller, achievementsManager.enemiesToGetDemonKiller - (achievementsManager.KilledEnemiesATALL + achievementsManager.killedEnemiesOnTheLevel)},
            {Achievements.GhostHunter, achievementsManager.enemiesToGetGhostHunter - (achievementsManager.KilledRedeersATALL + achievementsManager.killedRedeersOnTheLevel)},
            {Achievements.Exorcist, achievementsManager.enemiesToGetExorcism - (achievementsManager.KilledPossessedATALL + achievementsManager.killedPossessedOnTheLevel)},
            // Achievements.YouShallNotPass, none
            {Achievements.RichGuy, achievementsManager.spendToGetRichGuy - (achievementsManager.SpentMoneyATALL + achievementsManager.spentMoneyOnTheLevel)},
            {Achievements.Bersek, achievementsManager.killsWith1HPToGetBersek - (achievementsManager.KilledMobsWith1HPATALL - achievementsManager.killedMobsWith1HPOnTheLevel)},
            
        };

        patternALL = FindAnAchievementAll().ToString();
        patternLEFT = FindAnAchievementLeft().ToString();

        text.text = patternBASE + " " + patternALL + " " + ending + $"\n Remains: {patternLEFT} out of {patternALL}";
    }

    int FindAnAchievementAll()
    {
        return AtAllDick[achievement];
    }

    public int FindAnAchievementLeft()
    {
        if (LeftDick[achievement] > 0)
        {
            return LeftDick[achievement];
        }
        else
        {
            return 0;
        }
    }
    
    

    // Update is called once per frame
    void Update()
    {
    }
}