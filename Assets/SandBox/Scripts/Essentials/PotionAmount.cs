using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionAmount : MonoBehaviour
{
    public int amount;
    private string x = "x";
    private Text textWindow;
    public int maxAmount;
    //private AttackPotionReloading attackPotionReloading;
    //private ShieldPotionReloading shieldPotionReloading;
    private PlayerStats playerStats;
    private HealthBar healthBar;
    private float healthToGive;
    [SerializeField] private float percentFromMaxHealth;
    [SerializeField] private bool canUse;
    

    void Start()
    {
        healthBar = FindObjectOfType<HealthBar>();
        playerStats = FindObjectOfType<PlayerStats>();
        healthToGive = (playerStats.maxHelth / 100) * percentFromMaxHealth;
        textWindow = GetComponent<Text>();
        textWindow.text = x + amount;
        // gameManager = FindObjectOfType<GameManager>();
       // attackPotionReloading = FindObjectOfType<AttackPotionReloading>();
       // shieldPotionReloading = FindObjectOfType<ShieldPotionReloading>();
        TextUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (canUse)
        {
            if (amount == 0) return;
            if (Input.GetKeyDown(KeyCode.Alpha1) && CompareTag("AmountHealth"))
            {
              //  PlayerStats playerStats = FindObjectOfType<PlayerStats>();
                if (playerStats.currentHealth < playerStats.maxHelth && playerStats.currentHealth > 0)
                {
                    amount = Mathf.Clamp(amount - 1, 0, maxAmount);
                   // PlayerStats health = FindObjectOfType<PlayerStats>();
                    if (playerStats.maxHelth - playerStats.currentHealth < healthToGive)
                    {
                        playerStats.currentHealth = playerStats.maxHelth;
                        healthBar.SetHealth(playerStats.currentHealth);
                    }
                    else
                    {
                        playerStats.DecreaseHealth(-healthToGive);
                        healthBar.SetHealth(playerStats.currentHealth);
                        Debug.Log("health++++");
                    }

                    textWindow.text = x + amount;
                    print("+health");
                }
            }
            // else if (Input.GetKeyDown(KeyCode.Alpha2) && CompareTag("AmountPower") &&
            //          !attackPotionReloading.isAttackBuffed)
            // {
            //     amount = Mathf.Clamp(amount - 1, 0, maxAmount);
            //     attackPotionReloading.isAttackBuffed = true;
            //     textWindow.text = x + amount;
            //     print("+power");
            // }
            // else if (Input.GetKeyDown(KeyCode.Alpha3) && CompareTag("AmountShield") &&
            //          !shieldPotionReloading.isShieldBuffed)
            // {
            //     amount = Mathf.Clamp(amount - 1, 0, maxAmount);
            //     shieldPotionReloading.isShieldBuffed = true;
            //     textWindow.text = x + amount;
            //     print("+protect");
            // }
        }
    }

    public void TextUpdate()
    {
        if (!textWindow)
            Start();
        textWindow.text = x + amount; //Yeah like why not. Used in collectable script
    }
}