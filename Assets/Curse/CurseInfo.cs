using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CurseInfo
{
    public enum StackModes { Power, Levels, None }

    public string Id;
    public int LevelsLeft;
    public StackModes StackMode;


    public override string ToString()
    {
        return $"{Id}:{LevelsLeft}:{(int)StackMode}";
    }

    public static CurseInfo FromString(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;
        var items = text.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

        var curseInfo = new CurseInfo
        {
            Id = items[0],
            LevelsLeft = int.Parse(items[1]),
            StackMode = (StackModes)int.Parse(items[2])

        };
        return curseInfo;

    }
}
