using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCurse : CurseBase
{
    public float DecreasePercent = 50.0f;
    public override void Apply(Player player)
    {
        var _stats = player.gameObject.GetComponent<PlayerStats>();
        _stats.currentHealth *= 1 - DecreasePercent / 100;
        _stats.healthBar.SetHealth(_stats.currentHealth);
    }

    public override bool CanApply(Player p)
    {
        return true;
    }
}
