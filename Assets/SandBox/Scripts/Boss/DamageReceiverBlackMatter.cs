using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiverBlackMatter : MonoBehaviour
{
    private BlackMatter blackMatter;

    private void Start()
    {
        blackMatter = GetComponentInParent<BlackMatter>();
    }

    public void Damage(float[] attackDetails)
    {
        blackMatter.DamageReceive(attackDetails);
    }
}
