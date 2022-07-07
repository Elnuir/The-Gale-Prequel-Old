using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CurseRepo
{
    public const string CurseKey = "player-curses";

    public static CurseInfo[] Load()
    {
        var cursesString = PlayerPrefs.GetString(CurseKey);
        if (string.IsNullOrWhiteSpace(cursesString)) return new CurseInfo[0];

        string[] curses = cursesString.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        return curses.Select(CurseInfo.FromString).ToArray();
    }

    public static void Save(IReadOnlyList<CurseInfo> curses)
    {
        if (curses.Count > 0)
        {
            var cursesString = curses.Select(c => c.ToString()).Aggregate((acc, c) => acc + "," + c);
            PlayerPrefs.SetString(CurseKey, cursesString);
        }
        else
            PlayerPrefs.SetString(CurseKey, "");
    }
}
