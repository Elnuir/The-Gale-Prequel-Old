using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class BossMovement : NPathMovementBase
{
    private Rigidbody2D _physics;


    private Vector2 currVelocity;

    protected override void Start()
    {
        base.Start();
        _physics = GetComponent<Rigidbody2D>();
        // RESCAN GRID MAP ASTAR PATH
        var graphToScan = AstarPath.active.data.gridGraph;
        AstarPath.active.Scan(graphToScan);

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }


    public override bool CanMoveTo(Vector3 position)
    {
        return true;
    }

    public override void MoveTo(Vector3 position)
    {
        Destination = position;

        var delta = CurrentNode - transform.position;

        if (!IsPathNodeReached())
            _physics.velocity = Vector2.SmoothDamp(_physics.velocity, delta.normalized * Speed, ref currVelocity, 0.2f);
    }

    public override bool CanMoveToNow(Vector3 position)
    {
        return true;
    }
}