using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NibblerAttack : NEnemyAttack
{
    private ItdimkEnemyStateManager _state;

    public GameObject projectileFireBall;
    public Transform fireBallSpawnTransform;
    public float Cooldown = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        _state = GetComponent<ItdimkEnemyStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_state.CurrentState == ItdimkEnemyStateManager.NibblerState.RangedAttack)
        {
            RangedAttackAction();
        }
    }

    void RangedAttackAction()
    {
    }

    public override bool CanAttack(Transform target)
    {
        throw new System.NotImplementedException();
    }

    public override void Attack(Transform target)
    {
        if(!ActionEx.CheckCooldown(RangedAttackAction, Cooldown)) return;
        
        var p = projectileFireBall.GetCloneFromPool(null);
        p.transform.position = fireBallSpawnTransform.position;
        p.GetComponent<Fireball>().flyRight = transform.right.x > 0;
    }
}