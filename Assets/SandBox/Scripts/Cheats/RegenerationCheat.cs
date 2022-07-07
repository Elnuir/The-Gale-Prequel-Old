using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerationCheat : CheatBase
{
    public float Percent = 0.25f;
    public float Interval = 0.5f;

    private PlayerStats _stats;

    private void Start()
    {
        _stats = FindObjectOfType<PlayerStats>();
    }


    private void Update()
    {
        if (IsActive && _stats.currentHealth < _stats.maxHelth)
            if (ActionEx.CheckCooldown(Update, Interval))
            {

                _stats.currentHealth +=  (_stats.maxHelth - _stats.currentHealth) *  Percent;
                _stats.healthBar.SetHealth(_stats.currentHealth);
            }
    }
}
