using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ObscurityManager : MonoBehaviour
{
    [SerializeField] private GameObject[] dialogs;
    private ObscuritySubBaseManager obscuritySubBaseManager;
    private int amountOfHolyWater, amountOfHealthPotions, amountOfGold;
    private float amountOfHealth;

    public int NextSceneIndex;
    [SerializeField] private PotionAmount potionAmount; //FOR HEALTH POTIONS PURPOSE
    private Throw throw1; //FOR HOLY WATER PURPOSE
    private Score score; //FOR SCORE PURPOSE
    private PlayerStats playerStats; //FOR HEALTH PURPOSE
    private SettingsManager settingsManager;

    private Button[] buttons;

    void Start()
    {
        //FINDING SHIT
        settingsManager = FindObjectOfType<SettingsManager>();
        throw1 = FindObjectOfType<Throw>();
        score = FindObjectOfType<Score>();
        playerStats = FindObjectOfType<PlayerStats>();
        //potion amount = is being addressed directly

        amountOfHolyWater = throw1.amountSaintWater; //
        amountOfHealthPotions = potionAmount.amount; //
        amountOfGold = score.currentScore; // FOR COMPARISION PURPOSE
        amountOfHealth = playerStats.currentHealth; //

        //DONE FINDING SHIT
        var a = Random.Range(0, dialogs.Length);
        GameObject b = dialogs[a];
        b.SetActive(true);
        obscuritySubBaseManager = b.GetComponent<ObscuritySubBaseManager>();
        buttons = b.GetComponentsInChildren<Button>();
        int i = 0;
        foreach (var button in buttons)
        {
            Text text = button.GetComponentInChildren<Text>();
            if (obscuritySubBaseManager.healthToGive[i] != 0)
            {
                if (obscuritySubBaseManager.healthToGive[i] > 0)
                    text.text += $" [HP +{obscuritySubBaseManager.healthToGive[i]}] ";
                if (obscuritySubBaseManager.healthToGive[i] < 0)
                {
                    text.text += $" [HP {obscuritySubBaseManager.healthToGive[i]}] ";
                    if (Mathf.Abs(obscuritySubBaseManager.healthToGive[i]) > amountOfHealth)
                        button.interactable = false;
                }
            }

            if (obscuritySubBaseManager.goldToGive[i] != 0)
            {
                if (obscuritySubBaseManager.goldToGive[i] > 0)
                    text.text += $" [GOLD +{obscuritySubBaseManager.goldToGive[i]}] ";
                if (obscuritySubBaseManager.goldToGive[i] < 0)
                {
                    text.text += $" [GOLD {obscuritySubBaseManager.goldToGive[i]}] ";
                    if (Mathf.Abs(obscuritySubBaseManager.goldToGive[i]) > amountOfGold)
                        button.interactable = false;
                }
            }

            if (obscuritySubBaseManager.holyWaterToGive[i] != 0)
            {
                if (obscuritySubBaseManager.holyWaterToGive[i] > 0)
                    text.text += $" [WATER +{obscuritySubBaseManager.holyWaterToGive[i]}] ";
                if (obscuritySubBaseManager.holyWaterToGive[i] < 0)
                {
                    text.text += $" [WATER {obscuritySubBaseManager.holyWaterToGive[i]}] ";
                    if (Mathf.Abs(obscuritySubBaseManager.holyWaterToGive[i]) > amountOfHolyWater)
                        button.interactable = false;
                }
            }

            if (obscuritySubBaseManager.helPotToGive[i] != 0)
            {
                if (obscuritySubBaseManager.helPotToGive[i] > 0)
                {
                    text.text += $" [POTION +{obscuritySubBaseManager.helPotToGive[i]}] ";
                }

                if (obscuritySubBaseManager.helPotToGive[i] < 0)
                {
                    text.text += $" [POTION {obscuritySubBaseManager.helPotToGive[i]}] ";
                    if (Mathf.Abs(obscuritySubBaseManager.healthToGive[i]) > amountOfHealthPotions)
                    {
                        button.interactable = false;
                    }
                }
            }
            print(Mathf.Abs(obscuritySubBaseManager.healthToGive[i]));

            i++;
            // text.text = oldText + 
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Option0()
    {
        playerStats.currentHealth = Mathf.Clamp(playerStats.currentHealth + obscuritySubBaseManager.healthToGive[0], 0,
            playerStats.maxHelth);
        throw1.amountSaintWater += obscuritySubBaseManager.holyWaterToGive[0];
        score.currentScore += obscuritySubBaseManager.goldToGive[0];
        potionAmount.amount = Mathf.Clamp(potionAmount.amount + obscuritySubBaseManager.helPotToGive[0], 0, potionAmount.maxAmount);
        settingsManager.Save();
        SceneManager.LoadScene(NextSceneIndex);
    }

    public void Option1()
    {
        playerStats.currentHealth = Mathf.Clamp(playerStats.currentHealth + obscuritySubBaseManager.healthToGive[1], 0,
            playerStats.maxHelth);
        throw1.amountSaintWater += obscuritySubBaseManager.holyWaterToGive[1];
        score.currentScore += obscuritySubBaseManager.goldToGive[1];
        potionAmount.amount += Mathf.Clamp(potionAmount.amount + obscuritySubBaseManager.helPotToGive[1], 0, potionAmount.maxAmount);
        settingsManager.Save();
        SceneManager.LoadScene(NextSceneIndex);
    }

    public void Skip()
    {
        settingsManager.Save();
    }
}