using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoostCheat : CheatBase
{
    public float Amount;
    private PlayerCombatManager _combat;

    private void Start()
    {
        _combat = FindObjectOfType<PlayerCombatManager>();
    }

    protected override void ActivateCheat()
    {
        base.ActivateCheat();
        _combat.baseAttackDamage += Amount;
    }

    protected override void DeactivateCheat()
    {
        base.DeactivateCheat();
        _combat.baseAttackDamage -= Amount;
    }
}
