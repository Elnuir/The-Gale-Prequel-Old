using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestManager : MonoBehaviour
{
    private PlayerCombatManager combatManager;
    private PlayerStats playerStats;
    private SettingsManager settingsManager;
    float currentHealthOnRest;
    private float maxHealth;
    
    private float currentAttack;
    [SerializeField] Text currentAttackText;
    private float willBeAttack;
    [SerializeField] Text willBeAttackText;
    [SerializeField] int percentOfUpgradeAttack;
    [SerializeField] int percentOfHealth;
    private GameManager gameManager;
    void Start()
    {
        combatManager = FindObjectOfType<PlayerCombatManager>();
        playerStats = FindObjectOfType<PlayerStats>();
        settingsManager = FindObjectOfType<SettingsManager>();
        gameManager = FindObjectOfType<GameManager>();
        maxHealth = playerStats.maxHelth;
        currentHealthOnRest = playerStats.currentHealth;
        currentAttack = combatManager.baseAttackDamage;
        currentAttackText.text = (currentAttack*10f).ToString();

        willBeAttack = (float)Math.Round((percentOfUpgradeAttack / 100f + 1) * currentAttack, 1);
        willBeAttackText.text = (willBeAttack*10f).ToString();
    }
    public void UpdatePlayerAttack()
    {
        currentAttack = willBeAttack;
        combatManager.baseAttackDamage = currentAttack;
        gameManager.RunWinCanvas();
       // settingsManager.Save();
    }
    public void UpdateHealth()
    {
        currentHealthOnRest = (float)Math.Round( currentHealthOnRest + maxHealth * (percentOfHealth / 100f), 1);
        playerStats.currentHealth = currentHealthOnRest;
        gameManager.RunWinCanvas();
       // settingsManager.Save();
    }
}
