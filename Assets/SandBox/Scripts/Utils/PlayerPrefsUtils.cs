using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class PlayerPrefsUtils
{
    public enum ItemType { Float, Int, String }

    public static void DeleteAllBut((ItemType, string)[] exceptions)
    {
        var items = GetItems(exceptions);
        PlayerPrefs.DeleteAll();
        WriteItems(items);
    }

    private static Dictionary<string, object> GetItems((ItemType, string)[] items)
    {
        var result = new Dictionary<string, object>();

        foreach (var (type, key) in items)
            if (PlayerPrefs.HasKey(key))
                switch (type)
                {
                    case ItemType.Float:
                        result.Add(key, PlayerPrefs.GetFloat(key));
                        break;
                    case ItemType.Int:
                        result.Add(key, PlayerPrefs.GetInt(key));
                        break;
                    case ItemType.String:
                        result.Add(key, PlayerPrefs.GetString(key));
                        break;
                }
        return result;
    }

    private static void WriteItems(Dictionary<string, object> items)
    {
        foreach (var item in items)
        {
            if (item.Value is float)
                PlayerPrefs.SetFloat(item.Key, (float)item.Value);
            else if (item.Value is int)
                PlayerPrefs.SetInt(item.Key, (int)item.Value);
            else
                PlayerPrefs.SetString(item.Key, Convert.ToString(item.Value));
        }
    }
}