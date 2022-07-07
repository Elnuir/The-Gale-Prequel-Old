using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BossAttackState : AnimatedEntityState
{
    public UnityEvent PlayerReached;
    public TargetProviderBase Target;
    public LayerMask WhatIsGround;
    private bool isActivated;
    public float AttackSpeed = 40;
    private bool isHit = false;

    public override bool IsAvailable
    {
        get
        {
            if (isHit)
                return false;
            return base.IsAvailable;
        }
    }

    public override void ActivateState()
    {
        base.ActivateState();
        isActivated = true;
    }

    public override void DeactivateState()
    {
        base.DeactivateState();
        isActivated = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        isHit = false;

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isActivated)
        {
            isHit = true;
            PlayerReached?.Invoke();
        }
    }

    private void CheckHit()
    {
        var overlap = Physics2D.OverlapCircleAll(transform.position, 0.8f, WhatIsGround);

        if (overlap.Any(c => c.gameObject != gameObject))
        {
            isHit = true;
            PlayerReached?.Invoke();
        }
    }

    private void Update()
    {
        if (isActivated)
        {
            CheckHit();
            var direction = Target.GetTarget().position - transform.position;
            GetComponent<Rigidbody2D>().velocity = direction.normalized * AttackSpeed;
        }
    }


    public void ResetIsHit()
    {
        isHit = false;
    }
    
}
