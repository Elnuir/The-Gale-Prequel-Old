using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NEnemyHealth : MonoBehaviour
{
    public float DyingTime = 2f;
    public float HitTime = 0.7f;
    
    public abstract event Action OnHit;
    public abstract event Action OnDeath;
}
