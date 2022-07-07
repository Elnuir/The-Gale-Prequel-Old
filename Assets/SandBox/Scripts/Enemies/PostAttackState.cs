using UnityEngine;

public class PostAttackState : AnimatedEntityState
{
    public float DamageRadius = 3;
    public float Damage = 5;
    public float DamageCooldown = 0.2f;
    public float DurationMultiplier = 1;

    public LayerMask WhatIsPlayer;

    private Vector2 currVelocity;

    protected override void Update()
    {
        base.Update();

        if (IsActive)
        {
            var target = Physics2D.OverlapCircle(transform.position, DamageRadius, WhatIsPlayer);
            if (target)
            {
                Chase(target.transform);
                if (ActionEx.CheckCooldown(Update, DamageCooldown))
                    DealDamage(target.transform);
            }
        }
    }
    
    private void DealDamage(Transform target)
    {
        var attackDetails = new float[] {Damage, transform.position.x};
        target.SendMessage("Damage", attackDetails);
    }

    private void Chase(Transform target)
    {
        transform.position = Vector2.SmoothDamp(transform.position, target.position, ref currVelocity, 0.2f);
    }
}