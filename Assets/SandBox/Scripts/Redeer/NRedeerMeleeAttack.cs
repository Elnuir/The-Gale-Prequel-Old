using System;
using System.Collections.Generic;
using UnityEngine;

public class NRedeerMeleeAttack : NEnemyAttack
{
    public float Damage;
    public float Range;
    public float Cooldown;
    public bool IsFacingRight;

    protected Transform _target;

    public override bool CanAttack(Transform target)
    {
        if (!CheckFace(target) || Vector2.Distance(target.position, transform.position) > Range) return false;
        if (!((Func<Transform, bool>) CanAttack).CheckCooldown(Cooldown)) return false;
        return true;
    }

    public bool CheckFace(Transform target)
    {
        bool result;

        if (transform.eulerAngles.y < 100)
            result = target.transform.position.x >= transform.position.x;
        else
            result = target.transform.position.x <= transform.position.x;

        return IsFacingRight ? result : !result;
    }

    public override void Attack(Transform target)
    {
        var physics = GetComponent<Rigidbody2D>();
        physics.velocity = new Vector2(0, physics.velocity.y);

        _target = target;
    }

    public virtual void MeleeAttackAction()
    {
        if (_target == default) return;

        // var attackDetails = new float[2];
        // attackDetails[0] = Damage;
        // attackDetails[1] = transform.position.x;
// OLD DAMAGE
        var attackDetails = new AttackDetails()
        {
            Attacker = transform,
            attackerX = transform.position.x,
            damageAmount = Damage
        };
        _target.SendMessage("NewDamage", attackDetails);
    }
}