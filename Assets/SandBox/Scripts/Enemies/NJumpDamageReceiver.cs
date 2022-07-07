using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NJumpDamageReceiver : MonoBehaviour
{
    public float JumpDamageCooldown = 0.5f;

    public NEnemyHealth Health;
    public Transform BypassMessageTo;
    public Rect DamageArea = new Rect(-1, 1, 0.5f, 0.25f);

    public bool IsActive;
    
    private Rect Offset(Rect wallCheck)
    {
        var wallCheckOffset = this.DamageArea;
        wallCheckOffset.x *= Math.Sign(transform.right.x);
        wallCheckOffset.width *= Math.Sign(transform.right.x);
        wallCheckOffset.position += (Vector2) transform.position;
        return wallCheckOffset;
    }

    private void Start()
    {
        Health.OnDeath += () => { IsActive = false; };
    }

    // CALLS VIA MESSAGE
    public bool JumpDamage(float[] attackDetails)
    {
        var damageArea = Offset(DamageArea);
        if (Physics2D.OverlapArea(damageArea.min, damageArea.max))
        {
            if (IsActive && ActionEx.CheckCooldown((Func<float[], bool>) JumpDamage, JumpDamageCooldown))
            {
                BypassMessageTo.SendMessage("DamageReceive", attackDetails);
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        var damageArea = Offset(DamageArea);
        Gizmos.DrawWireCube(damageArea.center, damageArea.size);
    }
}