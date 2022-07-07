using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentLevel
{
    private static int _currentLevel;
    
    public static int CurrLevel
    {
        get => _currentLevel;
        set
        {
            _currentLevel = value;
            Debug.Log($"Current level is {value}");
        }
    }
    
}
