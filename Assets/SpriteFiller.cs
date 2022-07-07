using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpriteFiller : MonoBehaviour
{
    private Abilities abilities;
    private float fillAmount;

    private float FillAmount
    {
        get
        {
            //  if (abilityType == abilityTypeEnum.Shield)
            //{
              //fillAmount = abilities.shieldUSAGECoolDownBase;
            //}

            //if (abilityType == abilityTypeEnum.Grenage)
            //{
             //fillAmount = abilities.grenadeUSAGECoolDownBase;
            //}

            return fillAmount;
        }
        set
        {
            fillAmount = value;
        }
    }

    public enum abilityTypeEnum
    {
        Shield,
        Grenage
    }

    public abilityTypeEnum abilityType;
    void Start()
    {
        abilities = FindObjectOfType<Abilities>();
        if (abilities == null)
        {
            Debug.Log("Блять, филлер не нашел игрока, пиздец");
        }

        FillAmount = GetComponent<Image>().fillAmount;
    }

    // Update is called once per frame
    void Update()
    {
        AbilityCheck(abilityType.ToString());
    }

    void AbilityCheck(string ability)
    {
        if (ability == abilityTypeEnum.Shield.ToString() && abilities.IsShieldUSAGECoolingDown)
        {
            FillAmount = (abilities.shieldUSAGECoolDown -= Time.deltaTime) / 100f;
            print("Shhhh");
        }
    }
}
