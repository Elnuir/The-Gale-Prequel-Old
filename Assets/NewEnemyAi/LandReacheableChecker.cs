using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandReacheableChecker : ReacheableChecker
{
    public float MinDeltaX = 4f;
    public float MaxDeltaY = 5f;
    public PathMovementBaseX PathMovement;

    private void Start()
    {
        PathMovement ??= GetComponent<PathMovementBaseX>();
    }

    public override bool IsReacheable(Vector3 target)
    {
        return IsReacheablePrimary(target);
    }

    private bool IsReacheablePrimary(Vector3 target)
    {
        return !(Mathf.Abs(transform.position.x - target.x) < MinDeltaX
        && target.y - transform.position.y > MaxDeltaY);
    }
}

