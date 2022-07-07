using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinStopwatch : MonoBehaviour
{

    public void WroteToText(Text target)
    {
        var timeSpan = new TimeSpan(0, 0, 0, 0, (int) (Time.time * 1000));
        target.text = $"{timeSpan.Minutes}:{timeSpan.Seconds}";
    } 
}
