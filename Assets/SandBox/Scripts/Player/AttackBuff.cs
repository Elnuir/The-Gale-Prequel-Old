using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class AttackBuff : BuffBase
{
    public float Amount;
    private PlayerCombatManager manager;

    private void Start()
    {
        manager = FindObjectOfType<PlayerCombatManager>();
        Activate();
    }

    public override void Activate()
    {
        if(IsActive) return;
        base.Activate();
        manager.baseAttackDamage += Amount;
    }

    public override void Deactivate()
    {
        if (!IsActive) return;
        base.Deactivate();
        manager.baseAttackDamage -= Amount;
    }

}
