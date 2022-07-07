using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPossessedMeleeAttack : NRedeerMeleeAttack
{
    public bool IsVenomous;

    public override void MeleeAttackAction()
    {
        base.MeleeAttackAction();

        if (!IsVenomous || _target == null) return;

        var damager = _target.GetComponents<PeriodicDamage>().FirstOrDefault(c => c.DamagerId == PeriodicDamage.DamageTypes.Poison);
        
        if(damager != null)
            damager.Impose();
        else 
            Debug.Log("Не могу найти дамагера у игрока");

    }
}
