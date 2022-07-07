using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NRedeerRangedAttack : NEnemyAttack
{
    public GameObject projectileFireBall;
    public Transform fireBallSpawnTransform;

    public float MaxDistance;
    public float MaxDeltaY;
    public float Cooldown = 1f;
    public float MinDistance = 1f;
    
    public override bool CanAttack(Transform target)
    {
        float distance = Vector2.Distance(transform.position, target.position);
        
        if (distance < MinDistance || distance > MaxDistance) return false;
        if (Mathf.Abs(target.transform.position.y - transform.position.y) > MaxDeltaY) return false;
        if (target.position.x < transform.position.x &&  transform.right.x < 0 ) return false;
        if (target.position.x > transform.position.x && transform.right.x > 0 ) return false;
        return ((Func<Transform, bool>) CanAttack).CheckCooldown(Cooldown);
    }

    public override void Attack(Transform target)
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        ;
        // LOOK at throw fireball
    }

    // CALLS FROM ANIMATION
    public void ThrowFireball()
    {
        var p = projectileFireBall.GetCloneFromPool(null);
        p.transform.position = fireBallSpawnTransform.position;
        
        if (p.TryGetComponent(out FireballRedeer fireballR))
            fireballR.flyRight = transform.right.x > 0;

        if (p.TryGetComponent(out Fireball fb))
            fb.flyRight = transform.right.x > 0;
    }
}