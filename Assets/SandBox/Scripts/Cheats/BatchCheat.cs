using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatchCheat : CheatBase
{
    public CheatBase[] Cheats;

    protected override void ActivateCheat()
    {
        base.ActivateCheat();

        foreach (var c in Cheats)
        {
            if (!c.IsActive)
                c.SwitchActivity();
        }
    }

    protected override void DeactivateCheat()
    {
        base.DeactivateCheat();

        foreach (var c in Cheats)
        {
            if (c.IsActive)
                c.SwitchActivity();
        }
    }
}
