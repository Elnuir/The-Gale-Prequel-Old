using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealerManager : MonoBehaviour
{
    [SerializeField] Button attackBuy,
        maxHealthBuy,
        plusButtonHealthPotion,
       // plusButtonAttackPotion,
       // plusButtonShieldPotion,
        plusButtonSaintWater,
        addSlotHealthPotion,
       // addSlotAttackPotion,
        //addSlotShieldPotion,
        addSlotKnife; //addSlotSaintWater;
    //
    private Score coins;
    private PlayerCombatManager combatManager;
    private Player player;
    private PlayerStats playerStats;
    private GameObject healthBar;   //IMPORTANT THAT IT'S GAME OBJECT
    private Throw throw1;
    private KnivesCounter knivesCounter;
    private SettingsManager settingsManager;
    private AchievementsManager achievementsManager;

    private SaintWaterCounter saintWaterCounter;
    //
    private float currentMaxHealth;
    private float willBeMaxHealth;
    //
    //
    private float currentAttack;
    private float willBeAttack;

    //
    [SerializeField] int percentOfUpgradeAttack;
    [SerializeField] int percentOfMaxHealth;

    //PRICELIST
    [SerializeField]
    private int costOfAttackUpgrage,
        costOfMaxHealthUpgrade,
        costOfHealthPotion,
       // costOfAttackPotion,
       // costOfShieldPotion,
        costOfSaintWater,
        costOfSlotHealth,
       // costOfSlotAttack,
       // costOfSlotShield,
        costOfSlotKnife; //costOfSlotSaintWater;
    //[SerializeField] private Text costOfAttackTEXT, costOfMaxHealthTEXT;
    
    //OBJECTS POTIONS
    [SerializeField] PotionAmount healthPotionObject, attackPotionObject, shieldPotionObject;
    
    //TEXTS FOR ATTACK
    [SerializeField] private Text currentAttackText, willBeAttackText, currentMaxHealthText, willBeMaxHealthText,
        costAttackUpgradeText, costMaxHealthText,
        costHealthPotionText, //costAttackPotionText, costShieldPotionText,
        costHolyWaterText,
        costSlotHealthPotionText, //costSlotAttackPotionText, costSlotShieldPotionText,
        costSlotKnife;

    void Start()
    {
        coins = FindObjectOfType<Score>();
        combatManager = FindObjectOfType<PlayerCombatManager>();
        playerStats = FindObjectOfType<PlayerStats>();
        player = FindObjectOfType<Player>();
        healthBar = FindObjectOfType<HealthBar>().gameObject; //IMPORTANT THAT IT'S GAMEOBJECT
        throw1 = FindObjectOfType<Throw>();
        settingsManager = FindObjectOfType<SettingsManager>();
        knivesCounter = FindObjectOfType<KnivesCounter>();
        saintWaterCounter = FindObjectOfType<SaintWaterCounter>();
        achievementsManager = FindObjectOfType<AchievementsManager>();
        // addSlotSaintWater = GameObject.Find("AddSlotSaintWater").GetComponent<Button>();
        
        if (coins.currentScore <= costOfAttackUpgrage)
        {
            attackBuy.interactable = false;                  //ATTACK
        }
        if (coins.currentScore <= costOfMaxHealthUpgrade)
        {
            maxHealthBuy.interactable = false;                //HEALTH
        }
        if (coins.currentScore <= costOfHealthPotion || healthPotionObject.amount == healthPotionObject.maxAmount)
        {
            plusButtonHealthPotion.interactable = false;                //HEALTH POTION
        }
        else
        {
            plusButtonHealthPotion.interactable = true;
        }
        // if (coins.currentScore <= costOfAttackPotion || attackPotionObject.amount == attackPotionObject.maxAmount)
        // {
        //     plusButtonAttackPotion.interactable = false;                //ATTACK POTION
        // }
        // else
        // {
        //     plusButtonAttackPotion.interactable = true;
        // }
        // if (coins.currentScore <= costOfShieldPotion || shieldPotionObject.amount == shieldPotionObject.maxAmount)
        // {
        //     plusButtonShieldPotion.interactable = false;                //SHIELD POTION
        // }
        // else
        // {
        //     plusButtonShieldPotion.interactable = true;
        // }
        if (coins.currentScore <= costOfSaintWater)
        {
            plusButtonSaintWater.interactable = false;                //HOLY WATER
        }
        else
        {
            plusButtonSaintWater.interactable = true;
        }
        if (coins.currentScore <= costOfSlotHealth)
        {
            addSlotHealthPotion.interactable = false;                //SHIELD POTION
        }
        // if (coins.currentScore <= costOfSlotAttack)
        // {
        //     addSlotAttackPotion.interactable = false;                //SHIELD POTION
        // }
        // if (coins.currentScore <= costOfSlotShield)
        // {
        //     addSlotShieldPotion.interactable = false;                //SHIELD POTION
        // }
        if (coins.currentScore <= costOfSlotKnife || player.PlayerType == Player.TypeOfPlayer.Swordsman)
        {
            addSlotKnife.interactable = false;                //SLOTS KNIVES
        }
        
        currentAttack = combatManager.baseAttackDamage;
        currentAttackText.text = (currentAttack*10f).ToString();

        willBeAttack = (float)Math.Round((percentOfUpgradeAttack / 100f + 1) * currentAttack, 1);
        willBeAttackText.text = (willBeAttack*10f).ToString();
        
        currentMaxHealth = playerStats.maxHelth;
        currentMaxHealthText.text = (currentMaxHealth*10f).ToString();

        willBeMaxHealth = (float) Math.Round((percentOfMaxHealth / 100f + 1) * currentMaxHealth, 1);
        willBeMaxHealthText.text = (willBeMaxHealth*10f).ToString();
        
        //COSTS GFX ASSIGNMENT
        costAttackUpgradeText.text = "x" + costOfAttackUpgrage;
        costMaxHealthText.text = "x" + costOfMaxHealthUpgrade;
        costHealthPotionText.text = "x" + costOfHealthPotion;
        //costAttackPotionText.text = "x" + costOfAttackPotion;
      //  costShieldPotionText.text = "x" + costOfShieldPotion;
        costHolyWaterText.text = "x" + costOfSaintWater;
        //SLOTS
        costSlotHealthPotionText.text = "x" + costOfSlotHealth;
      //  costSlotAttackPotionText.text = "x" + costOfSlotAttack;
       // costSlotShieldPotionText.text = "x" + costOfSlotShield;
        costSlotKnife.text = "x" + costOfSlotKnife;
        //GameObject.Find("CostSlotSaintWater").GetComponent<Text>().text = "x" + costOfSlotSaintWater;

    }

    public void UpdatePlayerAttack()
    {
        currentAttack = willBeAttack;
        combatManager.baseAttackDamage = currentAttack;
        if (coins.currentScore >= costOfAttackUpgrage)
        {
            coins.currentScore = Mathf.Clamp(coins.currentScore - costOfAttackUpgrage, 0, Int32.MaxValue);
            achievementsManager.IncreaseSpentMoney(costOfAttackUpgrage);
            coins.UpdateScore();
            Start();
        }
    }
    public void UpdateMaxHealth()
    {
        currentMaxHealth = willBeMaxHealth;
        playerStats.maxHelth = currentMaxHealth;
        healthBar.GetComponent<Slider>().maxValue = playerStats.maxHelth;
        if (coins.currentScore >= costOfMaxHealthUpgrade)
        {
            coins.currentScore = Mathf.Clamp(coins.currentScore - costOfMaxHealthUpgrade, 0, Int32.MaxValue);
            achievementsManager.IncreaseSpentMoney(costOfMaxHealthUpgrade);
            coins.UpdateScore();
            Start();
        }
    }

    public void BuyHealthPotion()
    {
        if (coins.currentScore >= costOfHealthPotion && healthPotionObject.amount < healthPotionObject.maxAmount)
        {
            coins.currentScore = Mathf.Clamp(coins.currentScore - costOfHealthPotion, 0, Int32.MaxValue);
            achievementsManager.IncreaseSpentMoney(costOfHealthPotion);
            coins.UpdateScore();
            healthPotionObject.amount++;
            healthPotionObject.TextUpdate();
            Start();
        }
    }
    // public void BuyAttackPotion()
    // {
    //     if (coins.currentScore >= costOfAttackPotion && attackPotionObject.amount < attackPotionObject.maxAmount)
    //     {
    //         coins.currentScore = Mathf.Clamp(coins.currentScore - costOfAttackPotion, 0, Int32.MaxValue);
    //         coins.UpdateScore();
    //         attackPotionObject.amount++;
    //         attackPotionObject.TextUpdate();
    //         Start();
    //     }
    // }
    // public void BuyShieldPotion()
    // {
    //     if (coins.currentScore >= costOfShieldPotion && shieldPotionObject.amount < shieldPotionObject.maxAmount)
    //     {
    //         coins.currentScore = Mathf.Clamp(coins.currentScore - costOfShieldPotion, 0, Int32.MaxValue);
    //         coins.UpdateScore();
    //         shieldPotionObject.amount++;
    //         shieldPotionObject.TextUpdate();
    //         Start();
    //     }
    // }
    public void BuySaintWater()
    {
        if (coins.currentScore >= costOfSaintWater)
        {
            coins.currentScore = Mathf.Clamp(coins.currentScore - costOfSaintWater, 0, Int32.MaxValue);
            achievementsManager.IncreaseSpentMoney(costOfSaintWater);
            coins.UpdateScore();
            throw1.amountSaintWater++;
            saintWaterCounter.TextUpdate();
            Start();
        }
    }
    
    public void BuySlotHealth()
    {
        if (coins.currentScore >= costOfSlotHealth)
        {
            coins.currentScore = Mathf.Clamp(coins.currentScore - costOfSlotHealth, 0, Int32.MaxValue);
            achievementsManager.IncreaseSpentMoney(costOfSlotHealth);
            coins.UpdateScore();
            healthPotionObject.maxAmount++;
            Start();
        }
    }
    // public void BuySlotAttack()
    // {
    //     if (coins.currentScore >= costOfSlotAttack)
    //     {
    //         coins.currentScore = Mathf.Clamp(coins.currentScore - costOfSlotAttack, 0, Int32.MaxValue);
    //         coins.UpdateScore();
    //         attackPotionObject.maxAmount++;
    //         Start();
    //     }
    // }
    // public void BuySlotShield()
    // {
    //     if (coins.currentScore >= costOfSlotShield)
    //     {
    //         coins.currentScore = Mathf.Clamp(coins.currentScore - costOfSlotShield, 0, Int32.MaxValue);
    //         coins.UpdateScore();
    //         shieldPotionObject.maxAmount++;
    //         Start();
    //     }
    // }
    public void BuySlotKnives()
    {
        if (coins.currentScore >= costOfSlotKnife)
        {
            coins.currentScore = Mathf.Clamp(coins.currentScore - costOfSlotKnife, 0, Int32.MaxValue);
            achievementsManager.IncreaseSpentMoney(costOfSlotKnife);
            coins.UpdateScore();
            throw1.baseAmountOfKnives++;
            throw1.amountOfKnives = throw1.baseAmountOfKnives;
            knivesCounter.TextUpdate();
            Start();
        }
    }
    public void ContinueJorney()
    {
    //    settingsManager.Save();
    }
}
