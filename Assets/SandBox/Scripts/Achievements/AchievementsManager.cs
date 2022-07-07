using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementsManager : MonoBehaviour
{
    private AchievedChecker achievedChecker; //To light up texts in UI in pause menu
    [SerializeField] private AchievementHINT[] achievementHints; //To update LEFT number
    #region On The Level

    public int killedEnemiesOnTheLevel;
    public int killedNibblersOnTheLevel;
    public int killedRedeersOnTheLevel;
    public int killedPossessedOnTheLevel;

    public int spentMoneyOnTheLevel;

    public int killedMobsWith1HPOnTheLevel;

    #endregion

    #region AT ALL
    public int KilledEnemiesATALL
    {
        get => PlayerPrefs.GetInt("KilledEnemies", 0);
        set => PlayerPrefs.SetInt("KilledEnemies", value);
    }
    public int KilledNibblersATALL
    {
        get => PlayerPrefs.GetInt("KilledNibblers", 0);
        set => PlayerPrefs.SetInt("KilledNibblers", value);
    }
    public int KilledRedeersATALL
    {
        get => PlayerPrefs.GetInt("KilledRedeers", 0);
        set => PlayerPrefs.SetInt("KilledRedeers", value);
    }
    public int KilledPossessedATALL
    {
        get => PlayerPrefs.GetInt("KilledPossessed", 0);
        set => PlayerPrefs.SetInt("KilledPossessed", value);
    }
    public int SpentMoneyATALL
    {
        get => PlayerPrefs.GetInt("spentMoney", 0);
        set => PlayerPrefs.SetInt("spentMoney", value);
    }
    public int KilledMobsWith1HPATALL
    {
        get => PlayerPrefs.GetInt("KillsWith1HP", 0);
        set => PlayerPrefs.SetInt("KillsWith1HP", value);
    }
    
    #endregion

    #region Demon Killer Achievement

    public int enemiesToGetDemonKiller;

    private bool demonKillerIsShownOnce; //Used to check if an achievement was shown on the level just once

    [SerializeField]
    private GameObject demonKillerAchievementObject; //Actual UI on the scene to show the player with animation and shit

    public bool DemonKillerComparator
    {
        get => KilledEnemiesATALL + killedEnemiesOnTheLevel >=
               enemiesToGetDemonKiller; //Used to actually run an achievement during gameplay
    }

    public int DemonKillerIsShown
    {
        get => PlayerPrefs.GetInt("DemonKillerAchieved", 0); //Used to check if we have the achievement during gameplay MAIN SHIT YO
        set
        {
            if (DemonKillerComparator)
                PlayerPrefs.SetInt("DemonKillerAchieved",
                    1); //Used to save an achievement when we make save in SettingsManager
        }
    }

    #endregion

    #region Ghost Hunter Achievement

    public int enemiesToGetGhostHunter;

    private bool ghostHunterIsShownOnce; //Used to check if an achievement was shown on the level just once

    [SerializeField]
    private GameObject ghostHunterAchievementObject; //Actual UI on the scene to show the player with animation and shit

    public bool GhostHunterComparator
    {
        get => KilledRedeersATALL + killedRedeersOnTheLevel >=
               enemiesToGetGhostHunter; //Used to actually run an achievement during gameplay
    }

    public int GhostHunterIsShown
    {
        get => PlayerPrefs.GetInt("GhostHunterAchieved", 0); //Used to check if we have the achievement during gameplay
        set
        {
            if (GhostHunterComparator)
                PlayerPrefs.SetInt("GhostHunterAchieved", 1); //Used to save an achievement when we make save in SettingsManager
        }
    }

    #endregion
    
    #region Exorcism Achievement

    public int enemiesToGetExorcism;

    private bool exorcismIsShownOnce; //Used to check if an achievement was shown on the level just once

    [SerializeField]
    private GameObject exorcismAchievementObject; //Actual UI on the scene to show the player with animation and shit

    public bool ExorcismComparator
    {
        get => KilledPossessedATALL + killedPossessedOnTheLevel >=
               enemiesToGetExorcism; //Used to actually run an achievement during gameplay
    }

    public int ExorcismIsShown
    {
        get => PlayerPrefs.GetInt("ExorcismAchieved", 0); //Used to check if we have the achievement during gameplay
        set
        {
            if (ExorcismComparator)
                PlayerPrefs.SetInt("ExorcismAchieved", 1); //Used to save an achievement when we make save in SettingsManager
        }
    }

    #endregion

    #region You Shall Not Pass! Achievement

    public float damageHasBeenRecieved;
    
    private bool youShallNotPassIsShownOnce; //Used to check if an achievement was shown on the level just once
    private Player player;

    [SerializeField]
    private GameObject youShallNotPassAchievementObject; //Actual UI on the scene to show the player with animation and shit

    public bool YouShallNotPassComparator
    {
        get => damageHasBeenRecieved < 1 && SceneManager.GetActiveScene().name == "Boss1";
    }

    public int YouShallNotPassIsShown
    {
        get => PlayerPrefs.GetInt("YouShallNotPass", 0); //Used to check if we have the achievement during gameplay
        set
        {
            if (YouShallNotPassComparator)
                PlayerPrefs.SetInt("YouShallNotPass", 1); //Used to save an achievement when we make save in SettingsManager
        }
    }

    #endregion

    #region Rich Guy

    public int spendToGetRichGuy;

    private bool richGuyIsShownOnce; //Used to check if an achievement was shown on the level just once

    [SerializeField]
    private GameObject richGuyAchievementObject; //Actual UI on the scene to show the player with animation and shit

    public bool RichGuyComparator
    {
        get => SpentMoneyATALL + spentMoneyOnTheLevel >=
               spendToGetRichGuy; //Used to actually run an achievement during gameplay
    }

    public int RichGuyIsShown
    {
        get => PlayerPrefs.GetInt("RichGuyAchieved", 0); //Used to check if we have the achievement during gameplay
        set
        {
            if (RichGuyComparator)
                PlayerPrefs.SetInt("RichGuyAchieved", 1); //Used to save an achievement when we make save in SettingsManager
        }
    }

    #endregion

    #region Bersek

    public int killsWith1HPToGetBersek;
    private PlayerStats playerStats;

    private bool bersekIsShownOnce; //Used to check if an achievement was shown on the level just once

    [SerializeField]
    private GameObject bersekAchievementObject; //Actual UI on the scene to show the player with animation and shit

    public bool BersekComparator
    {
        get => KilledMobsWith1HPATALL + killedMobsWith1HPOnTheLevel >=
               killsWith1HPToGetBersek; //Used to actually run an achievement during gameplay
    }

    public int BersekIsShown
    {
        get => PlayerPrefs.GetInt("BersekAchieved", 0); //Used to check if we have the achievement during gameplay
        set
        {
            if (BersekComparator)
                PlayerPrefs.SetInt("BersekAchieved", 1); //Used to save an achievement when we make save in SettingsManager
        }
    }

    #endregion
    
    

    private void Start()
    {
        player = FindObjectOfType<Player>();
        playerStats = FindObjectOfType<PlayerStats>();
        
        // print("ВСЕГО УБИТО ПРОТИВНИКОВ  " + killedEnemiesATALL);
//        print("ВСЕГО УБИТО НИБЛЕРОВ  " + killedNibblersATALL);
        //print("ВСЕГО УБИТО ОДЕРЖИМЫХ  " + killedPossessedATALL);
        //   print("ВСЕГО УБИТО ПРИЗРАКОВ  " + killedRedeersATALL);
    }

    private void Update()
    {
        YouShallNotPassCheck();
    }

    #region Methods

    #region You Shall Not Pass

    void YouShallNotPassCheck()
    {
        if (player.knockback && damageHasBeenRecieved == 0)
        {
            damageHasBeenRecieved++;
        }
    }

    public void YouShallNotPass()
    {
        if (YouShallNotPassComparator && YouShallNotPassIsShown == 0 && !youShallNotPassIsShownOnce)
        {
            youShallNotPassAchievementObject.SetActive(true);
            YouShallNotPassIsShown = 0;
            youShallNotPassIsShownOnce = true;
        }
    }

    #endregion

    #region Rich Guy

    public void IncreaseSpentMoney(int spentMoney)
    {
        spentMoneyOnTheLevel += spentMoney;
        if (RichGuyComparator && RichGuyIsShown == 0 && !richGuyIsShownOnce)
        {
            richGuyAchievementObject.SetActive(true);
            richGuyIsShownOnce = true;
            RichGuyIsShown = 0;
            achievedChecker.Check();
        }
    }

    #endregion
    
    
    public void IncreaseKilledEnemiesTemp(string typeOfMob)
    {
        killedEnemiesOnTheLevel++;
        if (playerStats.currentHealth <= 10f)
        {
            killedMobsWith1HPOnTheLevel++;                      //FOR BERSEK
        }
        if (typeOfMob == nameof(AchievementHandler.MobType.Nibbler))
            killedNibblersOnTheLevel++;
        if (typeOfMob == nameof(AchievementHandler.MobType.Redeer))
            killedRedeersOnTheLevel++;
        if (typeOfMob == nameof(AchievementHandler.MobType.Possessed))
            killedPossessedOnTheLevel++;
        KillwiseAchievementsCheck();
        
    }
    private void KillwiseAchievementsCheck()
    {
        if (DemonKillerComparator && DemonKillerIsShown == 0 && !demonKillerIsShownOnce)
        {
            demonKillerAchievementObject.SetActive(true);
            demonKillerIsShownOnce = true;
            DemonKillerIsShown = 0;
            achievedChecker.Check();
        }
        if (GhostHunterComparator && GhostHunterIsShown == 0 && !ghostHunterIsShownOnce)
        {
            ghostHunterAchievementObject.SetActive(true);
            ghostHunterIsShownOnce = true;
            GhostHunterIsShown = 0;
            achievedChecker.Check();
        }
        if (ExorcismComparator && ExorcismIsShown == 0 && !exorcismIsShownOnce)
        {
            exorcismAchievementObject.SetActive(true);
            exorcismIsShownOnce = true;
            ExorcismIsShown = 0;
            achievedChecker.Check();
        }

        if (BersekComparator && BersekIsShown == 0 && !bersekIsShownOnce)
        {
            bersekAchievementObject.SetActive(true);
            bersekIsShownOnce = true;
            BersekIsShown = 0;
            achievedChecker.Check();
        }
    }

    #endregion
    
}