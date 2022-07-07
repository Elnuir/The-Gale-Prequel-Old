using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiverBasic : MonoBehaviour
{
    private NBasicEnemyHealth redeer;
    void Start()
    {
        redeer = GetComponentInParent<NBasicEnemyHealth>();
    }

    // For Calls via message
    public void Damage(float[] attackDetails)
    {
        redeer.DamageReceive(attackDetails);
    }
}
