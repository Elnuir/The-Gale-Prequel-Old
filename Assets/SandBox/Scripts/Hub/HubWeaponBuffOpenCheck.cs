using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubWeaponBuffOpenCheck : MonoBehaviour
{
    public static int[] isOpen = new int[1];

    static public void CheckIndexes()
    {
        isOpen[0] = PlayerPrefs.GetInt("FireBuff", 0);
    }

    static public void OpenFireBuff(int value)
    {
        PlayerPrefs.SetInt("FireBuff", value);
    }
}
