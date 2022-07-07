using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleCheat : CheatBase
{
    public float Multiplier = 2;


    protected override void ActivateCheat()
    {
        base.ActivateCheat();
        Time.timeScale = Multiplier;
    }

    protected override void DeactivateCheat()
    {
        base.DeactivateCheat();
        Time.timeScale = 1;
    }
}
