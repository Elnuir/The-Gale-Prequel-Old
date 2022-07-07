using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BossGoToAttackSpot))]
public class SlowdownBossState : AnimatedEntityState
{
    public float IsReachedDistance = 1;
    private BossGoToAttackSpot attackSpotGoing;

    protected override void Start()
    {
       base.Start();
       attackSpotGoing = GetComponent<BossGoToAttackSpot>();
    }
    
    public override bool IsAvailable
    {
        get
        {
            if (attackSpotGoing.CurrentSpot != default && Vector2.Distance(transform.position, attackSpotGoing.CurrentSpot) < IsReachedDistance)
                return false;
            return base.IsAvailable;
        } 
    }
}
