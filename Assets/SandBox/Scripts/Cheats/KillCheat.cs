using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCheat : CheatBase
{
    public float damageAmount = 50f;
    public override bool CanDeactivate => false;

    protected override void ActivateCheat()
    {
        base.ActivateCheat();
        Kill();

    }
    protected override void DeactivateCheat()
    {
        throw new InvalidOperationException();
    }

    private void Kill()
    {
        foreach (var o in FindObjectsOfType<AchievementHandler>())
        {
            float[] attackDetails = new[] { damageAmount, transform.position.x };
            o.transform.SendMessage("DamageReceive", attackDetails);

            if (o.EnemyType == AchievementHandler.MobType.Possessed)
                KillPossessed(o.transform);
        }
    }

    private void KillPossessed(Transform o)
    {
        if (o.TryGetComponent<NPossessedHealth>(out var health))
            if (health.isBelowExorcism)
                health.Dead(true);
    }
}
