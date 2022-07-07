using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HubWeaponOpenCheck    //HOLDS OPENED WEAPONS
{
   // public static int 0;
   // public static Dictionary<int, int> weapons = new Dictionary<int, int>()
   // {
   //     {0, 0},
   //     {1, 0},
   // };
  // public static int[] index = new int[2];
    public static int[] isOpen = new int[2];

    static public void CheckIndexes()
    {
        isOpen[0] = PlayerPrefs.GetInt("Sword", 1);
        isOpen[1] = PlayerPrefs.GetInt("Dagger", 0);
    }

    static public void OpenDagger(int value)
    {
        PlayerPrefs.SetInt("Dagger", value);
    }
    
    //public static int[]

   // public void Save()
   // {
   //     PlayerPrefs.SetInt("weapon-" + Name, isOpen ? 1 : 0);
   // }
}
