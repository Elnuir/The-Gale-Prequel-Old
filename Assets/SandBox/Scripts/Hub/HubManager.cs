using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HubManager : MonoBehaviour
{
    private SettingsManager settingsManager;
    [Serializable]
    public class WeaponInfo
    {
        public Button button;
        public int isOpen;
    }
    [Serializable]
    public class WeaponBuffInfo
    {
        public Button button;
        public int isOpen;
        public enum BuffType
        {
            Element, Storm
        }
        public BuffType typeOfBuff;
        
    }

    public WeaponInfo[] weapons;
    public WeaponBuffInfo[] weaponBuffs;
    void Start()
    {
        PlayerCombatManager playerCombatManager = FindObjectOfType<PlayerCombatManager>();
        playerCombatManager.canBeUsed = false;
        Throw throw1 = playerCombatManager.GetComponentInChildren<Throw>();
        throw1.canBeUsedKnives = false;
        throw1.canBeUsedWater = false;
        
        settingsManager = FindObjectOfType<SettingsManager>();
        HubWeaponOpenCheck.CheckIndexes();
        HubWeaponBuffOpenCheck.CheckIndexes();
        for (int i = 0; i < HubWeaponOpenCheck.isOpen.Length; i++)
        {
            weapons[i].isOpen = HubWeaponOpenCheck.isOpen[i];
            if (weapons[i].isOpen == 0)
            {
                weapons[i].button.interactable = false;
            }
        }
        for (int i = 0; i < HubWeaponBuffOpenCheck.isOpen.Length; i++)
        {
            weaponBuffs[i].isOpen = HubWeaponBuffOpenCheck.isOpen[i];
            if (weaponBuffs[i].isOpen == 0)
            {
                weaponBuffs[i].button.interactable = false;
            }
        }
    }

    // Update is called once per frame

    public void ChooseSwordsman()
    {
        settingsManager.playerIndex = 0;
        //settingsManager.Save();
    }
    public void ChooseDagger()
    {
        settingsManager.playerIndex = 1;
       // settingsManager.Save();
    }

}
