using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldPotionReloading : MonoBehaviour
{
    public float shieldBufftimeLeft = 30.0f;
    public float startBuffTime;
    public bool isShieldBuffed;
    public float percentToProtectFrom;

   // private PlayerStats playerStats;
    //private PlayerAttack attack;
    //private Throw knifeDamage;
    //public float percentFromDamageWillBePlused;
    //public float percentFromKnifeDamageWillBePlused;
    private Animation animation;

    private Image image;
  //  private GameManager gameManager;
    void Start()
    {
       // playerStats = FindObjectOfType<PlayerStats>();
        animation = GetComponentInParent<Animation>();
        startBuffTime = shieldBufftimeLeft;
       // knifeDamage = FindObjectOfType<Throw>();
        //attack = FindObjectOfType<PlayerAttack>();
        image = GetComponent<Image>();
        //gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    { 
        Buff();
        if (isShieldBuffed)
        {
            image.fillAmount = 1 - shieldBufftimeLeft / startBuffTime;
            Debug.Log(image.fillAmount);
        }
    }
    void Buff()                           //CALL ONLY THRU BOOL
    {
        if (isShieldBuffed)
        {
           // Debug.Log(shieldBufftimeLeft);
            shieldBufftimeLeft -= Time.deltaTime;
            // attack.attackDamage = (int)(attack.baseAttackDamage + attack.baseAttackDamage * percentFromDamageWillBePlused * 0.01f);
            //attack.attackDamage = attack.baseAttackDamage + attack.baseAttackDamage * percentFromDamageWillBePlused * 0.01f;
            //knifeDamage.attackDamage = knifeDamage.baseAttackDamage + knifeDamage.baseAttackDamage * percentFromKnifeDamageWillBePlused * 0.01f;
    
            if (shieldBufftimeLeft < 0)
            {
                animation.Play("FilledPotionPop");
                isShieldBuffed = false;
                //attack.attackDamage = attack.baseAttackDamage;
                //knifeDamage.attackDamage = knifeDamage.baseAttackDamage;
                shieldBufftimeLeft = startBuffTime;
            }
//            print(shieldBufftimeLeft);
        }
    }
}
