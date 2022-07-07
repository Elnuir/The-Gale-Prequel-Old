using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackPotionReloading : MonoBehaviour
{
    public float attackBufftimeLeft = 30.0f;
    public float startBuffTime;
    public bool isAttackBuffed;
    private PlayerCombatManager combatManager;
    private Throw knifeDamage;
    public float percentFromDamageWillBePlused;
    public float percentFromKnifeDamageWillBePlused;
    private Animation animation;

    private Image image;
  //  private GameManager gameManager;
    void Start()
    {
        animation = GetComponentInParent<Animation>();
        startBuffTime = attackBufftimeLeft;
        knifeDamage = FindObjectOfType<Throw>();
        combatManager = FindObjectOfType<PlayerCombatManager>();
        image = GetComponent<Image>();
        //gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    { 
        Buff();
        if (isAttackBuffed)
        {
            image.fillAmount = 1 - attackBufftimeLeft / startBuffTime;
        }
    }
    void Buff()                           //CALL ONLY THRU BOOL
    {
        if (isAttackBuffed)
        {
           // Debug.Log(attackBufftimeLeft);
            attackBufftimeLeft -= Time.deltaTime;
            // attack.attackDamage = (int)(attack.baseAttackDamage + attack.baseAttackDamage * percentFromDamageWillBePlused * 0.01f);
            combatManager.baseAttackDamage = combatManager.attackDamage + combatManager.attackDamage * percentFromDamageWillBePlused * 0.01f;
            knifeDamage.attackDamageKnife = knifeDamage.baseAttackDamageKnife + knifeDamage.baseAttackDamageKnife * percentFromKnifeDamageWillBePlused * 0.01f;
    
            if (attackBufftimeLeft < 0)
            {
                animation.Play("FilledPotionPop");
                isAttackBuffed = false;
                combatManager.baseAttackDamage = combatManager.attackDamage;
                knifeDamage.attackDamageKnife = knifeDamage.baseAttackDamageKnife;
                attackBufftimeLeft = startBuffTime;
            }
//            print(attackBufftimeLeft);
        }
    }
}
