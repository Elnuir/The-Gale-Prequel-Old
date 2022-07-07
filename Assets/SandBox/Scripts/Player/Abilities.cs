using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Abilities : MonoBehaviour
{
    private Player player;
    Throw throw1;
    private StaminaBar staminaBar;
    [SerializeField] float stamina;
    [SerializeField] private float maxStamina = 100;
    private float Stamina
    {
        get => stamina;
        set
        {
            stamina = value;
            staminaBar.SetStamina(stamina);
            
        }

    }

    #region Shield
    [SerializeField] float shieldUSAGECoolDownBase, shieldACTIVECountDownBase, shieldCost;
    bool isShieldUSAGECoolingDown, isShieldACTIVECountdown;
    float shieldUSAGECoolDown, shieldACTIVECountdown;
    
    // USED IN Combat manager
    public bool IsShieldUSAGECoolingDown
    {
        get => isShieldUSAGECoolingDown;
        set => isShieldUSAGECoolingDown = value;
    }
    private bool IsShieldACTIVECountDown
    {
        get => isShieldACTIVECountdown;
        set => isShieldACTIVECountdown = value;
    }
    #endregion

    #region StickyGrenade

    [SerializeField] float grenadeUSAGECoolDownBase, grenadeCost;
    private bool isGrenadeUSAGECoolingDown;
    private float grenadeUSAGECoolDown;
    private bool IsGrenadeUSAGECoolingDown
    {
        get => isGrenadeUSAGECoolingDown;
        set => isGrenadeUSAGECoolingDown = value;
    }

    #endregion
    
    
    void Start()
    {
        player = GetComponent<Player>();
        staminaBar = FindObjectOfType<StaminaBar>();
        throw1 = FindObjectOfType<Throw>();
        staminaBar.SetMaxStamina(maxStamina);
        
        shieldUSAGECoolDown = shieldUSAGECoolDownBase;
        shieldACTIVECountdown = shieldACTIVECountDownBase;

        grenadeUSAGECoolDown = grenadeUSAGECoolDownBase;
    }

    // Update is called once per frame
    void Update()
    {
        GetStaminaToItsMax();
        ActivateShieldAbility();
        ActivateGrenadeAbility();

    }

    #region ActivationAbilities

    void ActivateGrenadeAbility()
    {
        if (!IsGrenadeUSAGECoolingDown && Input.GetKeyDown(KeyCode.R) && grenadeCost < Stamina)
        {
            Debug.Log("Grenade");
            Invoke(nameof(SettingGrenageUSAGEToFalse), grenadeUSAGECoolDown);
            IsGrenadeUSAGECoolingDown = true;
            throw1.ShootGrenade();
            WithdrawStamina(grenadeCost);
        }
    }

    void ActivateShieldAbility()
    {
        if (!IsShieldUSAGECoolingDown && Input.GetKeyDown(KeyCode.E) && shieldCost < Stamina)
        {
            Debug.Log("enemy got damage");
            Invoke(nameof(SettingShieldCOOLDOWNToFalse), shieldUSAGECoolDown);
            Invoke(nameof(SettingShieldACTIVEToFalse), shieldACTIVECountdown);
            IsShieldUSAGECoolingDown = true;
            IsShieldACTIVECountDown = true;
            WithdrawStamina(shieldCost);
        }
    }

    #endregion
    
    #region SettingFalse

    #region Shield

    void SettingShieldCOOLDOWNToFalse()
    {
        IsShieldUSAGECoolingDown = false;
    }
    void SettingShieldACTIVEToFalse()
    {
        IsShieldACTIVECountDown = false;
    }

    #endregion
    
    #region Grenade

    void SettingGrenageUSAGEToFalse()
    {
        IsGrenadeUSAGECoolingDown = false;
    }

    #endregion
    
    #endregion
    void WithdrawStamina(float cost)
    {
        Stamina = Mathf.Clamp(Stamina - cost, 0f, maxStamina);
    }

    void GetStaminaToItsMax()
    {
        Stamina = Mathf.Clamp(Stamina + Time.deltaTime, 0f, maxStamina);
    }
    
}
