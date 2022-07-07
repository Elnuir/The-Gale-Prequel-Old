using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiverNibbler : MonoBehaviour
{
    private NBasicEnemyHealth nibbler;

    private void Start()
    {
        nibbler = GetComponentInParent<NBasicEnemyHealth>();
    }

    public void Damage(float[] attackDetails)
    {
        nibbler.DamageReceive(attackDetails);
    }
}
