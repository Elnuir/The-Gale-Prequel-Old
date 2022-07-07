using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private bool triggered;

    //[HideInInspector] public int attackDamageBeforeBuff;
    [SerializeField] private int howMuchGoldONLYFORGOLDCOLLECTABLE;
    private SaintWaterCounter saintWaterCounter;
    private Throw throw1;
   // private BuffSpawnManager buffSpawnManager;
    private void Start()
    {
        throw1 = FindObjectOfType<Throw>();
        saintWaterCounter = FindObjectOfType<SaintWaterCounter>();
     //   buffSpawnManager = FindObjectOfType<BuffSpawnManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerStats controller = other.GetComponent<PlayerStats>();
        //  PlayerAttack attack = other.GetComponent<PlayerAttack>();

//        var text = GameObject.Find("Text00").GetComponent<Text>();
        //text.text = "start";

        // TODO: debug
        if (controller != null && !triggered)
        {
            if (CompareTag("HealthCollectable"))
            {
                GameObject amountHealth = GameObject.Find("AmountHealth");                 //SPELL CORRECTLY!!!!
                PotionAmount healthamount = amountHealth.GetComponent<PotionAmount>();
               // GameObject healthAmount = GameObject.FindGameObjectWithTag("AmountHealth");

                

               // if (healthAmount.GetComponent<PotionAmount>().amount < healthAmount.GetComponent<PotionAmount>().maxAmount) 
               if(healthamount.amount < healthamount.maxAmount)
                {
                    if (healthamount.CompareTag("AmountHealth"))
                    {
                       // text.text += "last if";
                        triggered = true;
                        healthamount.amount = Mathf.Clamp(healthamount.amount + 1, 0, healthamount.maxAmount);      //HEALTH
                        healthamount.TextUpdate();
                        //  healthAmount.GetComponent<PotionAmount>().amount = Mathf.Clamp(healthAmount.GetComponent<PotionAmount>().amount + 1, 0, healthAmount.GetComponent<PotionAmount>().maxAmount);
                        // healthAmount.GetComponent<PotionAmount>().TextUpdate();
                    //    buffSpawnManager.activeBuffs--;
                        Destroy(gameObject);

                        //text.text += "zz";
                    }
                }
            }
            else if (CompareTag("GoldCollectable"))
            {
                triggered = true;
                var score = FindObjectOfType<Score>();
                score.currentScore += howMuchGoldONLYFORGOLDCOLLECTABLE; //GOLD
                score.UpdateScore();
              //  buffSpawnManager.activeBuffs--;
                Destroy(gameObject);
            }
            else if (CompareTag("AttackBuffCollectable"))
            {
                GameObject powerAmount = GameObject.FindGameObjectWithTag("AmountPower");
                if (powerAmount.GetComponent<PotionAmount>().amount <
                    powerAmount.GetComponent<PotionAmount>().maxAmount)
                {
                    triggered = true;
                    powerAmount.GetComponent<PotionAmount>().amount = Mathf.Clamp(
                        powerAmount.GetComponent<PotionAmount>().amount + 1, 0,
                        powerAmount.GetComponent<PotionAmount>().maxAmount);
                    powerAmount.GetComponent<PotionAmount>().TextUpdate();
                  //  buffSpawnManager.activeBuffs--;
                    Destroy(gameObject);
                    //print("There's");
                }
            }
            else if (CompareTag("ShieldCollectable"))
            {
                GameObject shieldAmount = GameObject.FindGameObjectWithTag("AmountShield");
                if (shieldAmount.GetComponent<PotionAmount>().amount <
                    shieldAmount.GetComponent<PotionAmount>().maxAmount)
                {
                    triggered = true;
                    shieldAmount.GetComponent<PotionAmount>().amount = Mathf.Clamp(
                        shieldAmount.GetComponent<PotionAmount>().amount + 1, 0,
                        shieldAmount.GetComponent<PotionAmount>().maxAmount);
                    shieldAmount.GetComponent<PotionAmount>().TextUpdate();
                 //   buffSpawnManager.activeBuffs--;
                    Destroy(gameObject);
                    print("There'sShield");
                }
            }
            else if (CompareTag("SaintWaterCollectable"))
            {
               // GameObject saintWaterAmount = GameObject.FindGameObjectWithTag("AmountSaintWater");
                //if (shieldAmount.GetComponent<PotionAmount>().amount <
                 //   shieldAmount.GetComponent<PotionAmount>().maxAmount)
               // {
                    triggered = true;
                    throw1.amountSaintWater++;
                    saintWaterCounter.TextUpdate();
//                    buffSpawnManager.activeBuffs--;
                    Destroy(gameObject);
                    print("There's water");
               // }
            }
        }
    }
}