using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiverPossessed : MonoBehaviour
{
    private NPossessedHealth possessed;
    void Start()
    {
        possessed = GetComponentInParent<NPossessedHealth>();
    }

    public void Damage(float[] attackDetails)
    {
        possessed.DamageReceive(attackDetails);
    }
}
