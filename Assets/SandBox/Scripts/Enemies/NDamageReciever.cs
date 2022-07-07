using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NDamageReciever : MonoBehaviour
{
    public Transform BypassMessageTo;


    // CALLS VIA MESSAGE
    public bool Damage(float[] attackDetails)
    {
        BypassMessageTo.SendMessage("DamageReceive", attackDetails);
        return true;
    }
    
}
