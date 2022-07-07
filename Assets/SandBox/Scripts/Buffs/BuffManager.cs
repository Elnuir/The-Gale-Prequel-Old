using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public string[] DummyBuffs = new string[0];
    public ImposableBuff[] ImposableBuffs = new ImposableBuff[0];

    public bool CheckActive(string buffId)
    {
        if (!IsRegistered(buffId))
            Debug.Log($"{buffId} is not registered!");
        return PlayerPrefs.GetInt($"buff-{buffId}", 0) != 0;
    }

    public void Activate(string buffId)
    {
        if (!IsRegistered(buffId))
            Debug.Log($"{buffId} is not registered!");
        else
            PlayerPrefs.SetInt($"buff-{buffId}", 1);
    }

    public void Dectivate(string buffId)
    {
        if (!IsRegistered(buffId))
            Debug.Log($"{buffId} is not registered!");
        else
            PlayerPrefs.SetInt($"buff-{buffId}", 0);
    }

    private bool IsRegistered(string buffId)
    {
        return DummyBuffs.Contains(buffId) || ImposableBuffs.Any(b => b.BuffId == buffId);
    }
}
