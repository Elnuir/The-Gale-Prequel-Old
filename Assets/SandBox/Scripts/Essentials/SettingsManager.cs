using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private LevelController levelController;
    public UnityEvent levelCompleted;

    //CHARACTERISTICS OF THE CHARACTER
    private float baseAttackDamage
    {
        get => PlayerPrefs.GetFloat("baseAttackDamage", 40);
        set { PlayerPrefs.SetFloat("baseAttackDamage", value); }
    }

    private float maxHealth
    {
        get => PlayerPrefs.GetFloat("maxHealth", 100);
        set { PlayerPrefs.SetFloat("maxHealth", value); }
    }

    //SLOTS FOR POTIONS
    private int slotsHealthPotion
    {
        get => PlayerPrefs.GetInt("slotsHealthPotion", 3);
        set { PlayerPrefs.SetInt("slotsHealthPotion", value); }
    }

    // private int slotsAttackPotion
    // {
    //     get => PlayerPrefs.GetInt("slotsAttackPotion", 3);
    //     set
    //     {
    //         PlayerPrefs.SetInt("slotsAttackPotion", value);
    //         PlayerPrefs.Save();
    //     }
    // }
    // private int slotsShieldPotion
    // {
    //     get => PlayerPrefs.GetInt("slotsShieldPotion", 3);
    //     set
    //     {
    //         PlayerPrefs.SetInt("slotsShieldPotion", value);
    //     }
    // }
    //SLOTS FOR KNIVES
    private int slotsKnives
    {
        get => PlayerPrefs.GetInt("slotsKnives", 5);
        set { PlayerPrefs.SetInt("slotsKnives", value); }
    }
    //EXPOSABLE

    private int healthPotionsAmount
    {
        get => PlayerPrefs.GetInt("AmountHealth", 2);
        set
        {
            PlayerPrefs.SetInt("AmountHealth", value);
            healthPotionOBJECT.TextUpdate();
        }
    }

    // private int attackPotionsAmount
    // {
    //     get => PlayerPrefs.GetInt("AmountPower", 2);
    //     set
    //     {
    //         PlayerPrefs.SetInt("AmountPower", value);
    //         attackPotionOBJECT.TextUpdate();
    //     }
    // }

    // private int shieldPotionsAmount
    // {
    //     get => PlayerPrefs.GetInt("AmountShield", 2);
    //
    //     set
    //     {
    //         PlayerPrefs.SetInt("AmountShield", value);
    //         shieldPotionOBJECT.TextUpdate();
    //     }
    // }
    private int currentscore
    {
        get => PlayerPrefs.GetInt("full-score", 0);
        set
        {
            PlayerPrefs.SetInt("full-score", value);
            FindObjectOfType<Score>().UpdateScore();
        }
    }

    private float currenthealth
    {
        get => PlayerPrefs.GetFloat("currentHealth", FindObjectOfType<PlayerStats>().maxHelth);
        set
        {
            PlayerPrefs.SetFloat("currentHealth", value);
            FindObjectOfType<HealthBar>().SetHealth(currenthealth);
        }
    }

    private float healthBarSliderMaxValue
    {
        get => PlayerPrefs.GetFloat("maxValueHealthSlider", FindObjectOfType<PlayerStats>().maxHelth);
        set { PlayerPrefs.SetFloat("maxValueHealthSlider", value); }
    }

    private int saintWaterAmount
    {
        get => PlayerPrefs.GetInt("saintWaterAmount", 0);
        set
        {
            PlayerPrefs.SetInt("saintWaterAmount", value);
            FindObjectOfType<SaintWaterCounter>().TextUpdate();
        }
    }

    private int possessedTutorialPassed
    {
        get => PlayerPrefs.GetInt("PossessedTutorialPassed", 0);
        set { PlayerPrefs.SetInt("PossessedTutorialPassed", value); }
    }

    public int playerIndex
    {
        get => PlayerPrefs.GetInt("playerIndex", 0);
        set
        {
            PlayerPrefs.SetInt("playerIndex", value);
            // PlayerPrefs.Save();
        }
    }


    // private int knivesAmount
    // {
    //     get => PlayerPrefs.GetInt("AmountKnives", 5);
    //     set
    //     {
    //         PlayerPrefs.SetInt("AmountKnives", value);
    //         GameObject.FindGameObjectWithTag("AmountKnives").GetComponent<KnivesCounter>().TextUpdate();
    //     }
    // }
    [SerializeField] private PotionAmount healthPotionOBJECT, attackPotionOBJECT, shieldPotionOBJECT;
    [SerializeField] private Score score;
    private PlayerCombatManager combatManager;
    private PlayerStats playerStats;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameManager gameManager;
    private Throw throw1;
    private SaintWaterCounter saintWaterCounter;
    private PossessedTutorialCheck possessedTutorialCheck;
    private AchievementsManager achievementsManager;

    // private void OnEnable()
    // {
    //     Invoke("Start", 3.5f);
    //
    // }

    // Start is called before the first frame update
    void Awake()
    {
        //SPAWNING CHARACTER
        gameManager.SpawningCharacterOnStart(playerIndex); //IMPORTANT TO BE ON TOP
        
        //FINDING SHIT
        levelController = FindObjectOfType<LevelController>();
        achievementsManager = FindObjectOfType<AchievementsManager>();
        //score = FindObjectOfType<Score>();
        throw1 = FindObjectOfType<Throw>();
        saintWaterCounter = FindObjectOfType<SaintWaterCounter>();
        combatManager = FindObjectOfType<PlayerCombatManager>();
        playerStats = FindObjectOfType<PlayerStats>();
        possessedTutorialCheck = FindObjectOfType<PossessedTutorialCheck>();
        //healthBar = FindObjectOfType<HealthBar>();
        //GETTING SAVED CHARACTERISTICS

        combatManager.baseAttackDamage = baseAttackDamage;
        playerStats.maxHelth = maxHealth;
        healthBar.GetComponent<Slider>().maxValue = healthBarSliderMaxValue;

        //GETTING SAVED SLOTS POTIONS

        healthPotionOBJECT.maxAmount = slotsHealthPotion;
        // attackPotionOBJECT.maxAmount = slotsAttackPotion;
        //shieldPotionOBJECT.maxAmount = slotsShieldPotion;

        //GETTING SAVED SLOTS KNIVES
        throw1.baseAmountOfKnives = slotsKnives;

        //GETTING SAVED AMOUNTS OF POTIONS

        PotionAmount[] potionAmounts = FindObjectsOfType<PotionAmount>();
        healthPotionOBJECT.amount = healthPotionsAmount;
        //attackPotionOBJECT.amount = attackPotionsAmount;
        //shieldPotionOBJECT.amount = shieldPotionsAmount;

        foreach (var potionamount in potionAmounts)
        {
            potionamount.TextUpdate();
        }

        //GETTING SAVED AMOUNT OF COINS

        score.currentScore = currentscore;
        score.UpdateScore();

        //GETTING SAVED AMOUNT OF HEALTH
        playerStats.currentHealth = currenthealth;
        healthBar.SetHealth(currenthealth);

        //GETTING SAVED AMOUNT OF HOLY WATER
        throw1.amountSaintWater = saintWaterAmount;
        saintWaterCounter.TextUpdate();

        //GETTING TUTORIALS STATES
        possessedTutorialCheck.tutorIsDone = possessedTutorialPassed;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            GameManager.gameIsPaused = false;
            FindObjectOfType<GameManager>().RunWinCanvas();
        }

        if (Input.GetKey(KeyCode.O))
        {
            score.currentScore += 100;
            score.UpdateScore();
        }

        if (Input.GetKey(KeyCode.P))
        {
            playerStats.currentHealth = Mathf.Clamp(playerStats.currentHealth + 20f, 0, playerStats.maxHelth);
            healthBar.SetHealth(playerStats.currentHealth);
        }

        if (Input.GetKey(KeyCode.L))
        {
            throw1.amountOfKnives += 10;
        }

        if (Input.GetKey(KeyCode.U))
        {
            throw1.amountSaintWater += 10;
        }
    }

    private void DisableCheats()
    {
        var cheats = FindObjectsOfType<CheatBase>();
        foreach (var cheat in cheats)
        {
            if (cheat.IsActive && cheat.CanDeactivate)
                cheat.SwitchActivity();
        }
        var buffs = FindObjectsOfType<BuffBase>();
        foreach (var buff in buffs)
            buff.Deactivate();
    }

    public void Save()
    {
        DisableCheats();

        //SAVING PLAYER CHARACTERISTICS
        baseAttackDamage = combatManager.baseAttackDamage;
        maxHealth = playerStats.maxHelth;
        healthBarSliderMaxValue = playerStats.maxHelth;

        //SAVING SLOTS POTIONS
        slotsHealthPotion = healthPotionOBJECT.maxAmount;
        // slotsAttackPotion = attackPotionOBJECT.maxAmount;
        // slotsShieldPotion = shieldPotionOBJECT.maxAmount;

        //SAVING SLOTS KNIVES
        slotsKnives = throw1.baseAmountOfKnives;


        //SAVING ALL SCORE, POTIONS

        //POTIONS
        healthPotionsAmount = healthPotionOBJECT.amount;
        // attackPotionsAmount = attackPotionOBJECT.amount;
        // shieldPotionsAmount = shieldPotionOBJECT.amount;

        //SCORE

        currentscore = score.currentScore;

        //HEALTH

        currenthealth = playerStats.currentHealth;

        //AMOUNT OF HOLY WATER

        saintWaterAmount = throw1.amountSaintWater;

        //TUTORIALS STATES
        possessedTutorialPassed = possessedTutorialCheck.tutorIsDone;

        //ACHIEVEMENTS

        achievementsManager.KilledEnemiesATALL += achievementsManager.killedEnemiesOnTheLevel;
        achievementsManager.KilledNibblersATALL += achievementsManager.killedNibblersOnTheLevel;
        achievementsManager.KilledRedeersATALL += achievementsManager.killedRedeersOnTheLevel;
        achievementsManager.KilledPossessedATALL += achievementsManager.killedPossessedOnTheLevel;
        achievementsManager.SpentMoneyATALL += achievementsManager.spentMoneyOnTheLevel;
        achievementsManager.KilledMobsWith1HPATALL += achievementsManager.killedMobsWith1HPOnTheLevel;
        //RESET ALL "ON LEVEL" TO AVOID ADDITION 2 SAME VALUES TO EACH OTHER "SHIT WITH POSSESSED - 12+12 AND HE GIVES YOU AN ACHIEVEMENT"
        achievementsManager.killedEnemiesOnTheLevel = 0;
        achievementsManager.killedNibblersOnTheLevel = 0;
        achievementsManager.killedRedeersOnTheLevel = 0;
        achievementsManager.killedPossessedOnTheLevel = 0;
        achievementsManager.spentMoneyOnTheLevel = 0;
        achievementsManager.killedMobsWith1HPOnTheLevel = 0;
        //ACTUALLY SAVING ACHIEVEMENTS AFTER LEVEL COMPLETE, THUS IT WILL BE FOREVER ACHIEVEMENT, NOT ONLY ON A LEVEL
       // achievementsManager.DemonKillerIsShown = 0;
      //  achievementsManager.GhostHunterIsShown = 0;
        //achievementsManager.ExorcismIsShown = 0;
       // achievementsManager.YouShallNotPassIsShown = 0;
       // achievementsManager.RichGuyIsShown = 0;
       // achievementsManager.BersekIsShown = 0;

        levelCompleted?.Invoke();
        levelController.CompleteLevel(CurrentLevel.CurrLevel);
     //   SceneManager.LoadScene("MainMenu");
    }
}