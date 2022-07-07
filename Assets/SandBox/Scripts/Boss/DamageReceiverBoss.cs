using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiverBoss : MonoBehaviour
{
    private Boss boss;
    private void Start()
    {
        boss = GetComponentInParent<Boss>();
    }

    public void Damage(float[] attackDetails)
    {
        boss.DamageReceive(attackDetails);
    }
}
