using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MovementStats))]
public class NNiblerMovement : NPathMovementBase
{
    public LayerMask WhatIsGround;
    public LayerMask EnemiesLayer;
    public float NeighborDistance = 3.2f;

    private Rigidbody2D _physics;

    public Rect WallCheck;
    public Rect AirCheck;
    public Rect CeilingCheck;
    private MovementStats _movementStats;

    private Vector2 currVelocity;

    protected override void Start()
    {
        _physics = GetComponent<Rigidbody2D>();
        _movementStats = GetComponent<MovementStats>();

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        FlipFix();
    }


    private bool InFrontEnemuesCheck()
    {
        var objs = Physics2D.OverlapCircleAll(transform.position, NeighborDistance, EnemiesLayer);

        foreach (var obj in objs)
            if (obj.TryGetComponent<AchievementHandler>(out var mobType))
            {
                if (obj.transform == transform) continue;
                if (mobType.EnemyType != AchievementHandler.MobType.Nibbler && mobType.EnemyType != AchievementHandler.MobType.Possessed) continue;

                if (Vector2.Distance(transform.position, Destination) > Vector2.Distance(obj.transform.position,
                    obj.GetComponent<NNiblerMovement>().Destination) + 0.2f)
                    return true;
                // if (Mathf.Abs(transform.right.x - obj.transform.right.x) < 0.1f)
                // {
                //     if (IsInFront(obj.transform)) return true;
                // }

            }

        return false;
    }

    // private bool IsInFront(Transform obj)
    // {
    //     if (obj.GetComponent<Rigidbody2D>().velocity.magnitude > 0.3f)
    //         return obj.GetInstanceID() > transform.GetInstanceID();
    //     else
    //         return false;

    //     if (transform.right.x > 0)
    //         return obj.transform.position.x > transform.position.x;
    //     else
    //         return obj.transform.position.x < transform.position.x;
    // }

    public override bool CanMoveTo(Vector3 position)
    {
        if (InFrontEnemuesCheck())
        {
            var currVelocity = _physics.velocity;
            _physics.velocity = new Vector2(0, currVelocity.y);
            return false;
        }

        Destination = position;
        if (TargetPath == default || TargetPath.vectorPath.Count < 2) return true;

        int maxCount = 0;
        int count = 0;
        for (int i = 1; i < TargetPath.vectorPath.Count; ++i)
        {
            var delta = TargetPath.vectorPath[i].y - TargetPath.vectorPath[i - 1].y;
            var deltaX = TargetPath.vectorPath[i].x - TargetPath.vectorPath[i - 1].x;
            var angle = Mathf.Atan2(delta, deltaX) * Mathf.Rad2Deg;
            if (angle > 90 - 30 && angle < 90 + 30)
                count++;
            else
            {
                if (count > maxCount)
                    maxCount = count;
                count = 0;
            }
        }

        if (count > maxCount)
            maxCount = count;

        return maxCount <= 3;
    }

    public override void MoveTo(Vector3 position)
    {
        var currVelocity = _physics.velocity;

        // if (InFrontEnemuesCheck())
        // {
        //     _physics.velocity = new Vector2(0, currVelocity.y);
        //     return;
        // }

        if (!_movementStats.IsGrounded)
        {
            _physics.velocity = new Vector2(transform.right.x * Speed, _physics.velocity.y);
        }


        Destination = position;
        if (Vector2.Distance(position, transform.position) < NodeRachedDistance) return;

        var wp = CurrentNode;

        if (wp.x > transform.position.x)
            _physics.velocity = Vector2.SmoothDamp(currVelocity, new Vector2(Speed, currVelocity.y),
                ref this.currVelocity, 0.1f);
        else
            _physics.velocity = Vector2.SmoothDamp(currVelocity, new Vector2(-Speed, currVelocity.y),
                ref this.currVelocity, 0.1f);

        if (ShouldJump())
        {
            _physics.AddForce(new Vector2(0, 25), ForceMode2D.Impulse);
        }
    }

    public bool ShouldJump()
    {
        if (!_movementStats.IsGrounded) return false;

        var wallCheckOffset = Offset(WallCheck);

        Collider2D wallCheck = Physics2D.OverlapArea(wallCheckOffset.min, wallCheckOffset.max,
            WhatIsGround);

        if (wallCheck != default) return ((Func<bool>)ShouldJump).CheckCooldown(0.5f);

        // var airCheckOffset = Offset(AirCheck);

        // Collider2D airCheck = Physics2D.OverlapArea(airCheckOffset.min, airCheckOffset.max,
        //     WhatIsGround);

        // if (airCheck == default)
        // {
        //     var ceilingCheckOffset = Offset(CeilingCheck);

        //     Collider2D ceilingCheck = Physics2D.OverlapArea(ceilingCheckOffset.min, ceilingCheckOffset.max,
        //         WhatIsGround);

        //     if (ceilingCheck == default)
        //         return ((Func<bool>) ShouldJump).CheckCooldown(0.4f);
        // }

        return false;
    }

    void FlipFix()
    {

        if (Vector2.Distance(Destination, transform.position) < NodeRachedDistance)
        {
            if (Mathf.Sign(Destination.x - transform.position.x) != -Mathf.Sign(transform.right.x))
            {
                transform.Rotate(0, 180, 0);
            }
        }

        if (Mathf.Abs(_physics.velocity.x) < 0.3f) return;

        if (_physics.velocity.x > 0 != transform.right.x < 0)
        {
            if (ActionEx.CheckCooldown(FlipFix, 0.1f))
                transform.Rotate(0, 180, 0);
        }
    }

    private void OnDrawGizmos()
    {
        var wallCheckOffset = Offset(WallCheck);


        Collider2D wallCheck = Physics2D.OverlapArea(wallCheckOffset.min, wallCheckOffset.max,
            WhatIsGround);
        if (wallCheck)
            Gizmos.color = Color.red;

        Gizmos.DrawWireCube(wallCheckOffset.center,
            wallCheckOffset
                .size); //wallCheckA.transform.position, wallCheck.transform.position + transform.right * wallCheckDistance);
        var airCheckOffset = Offset(AirCheck);
        Gizmos.DrawWireCube(airCheckOffset.center,
            airCheckOffset
                .size); //wallCheckA.transform.position, wallCheck.transform.position + transform.right * wallCheckDistance);

        var ceilingCheckOffset = Offset(CeilingCheck);
        Gizmos.DrawWireCube(ceilingCheckOffset.center,
            ceilingCheckOffset
                .size); //wallCheckA.transform.position, wallCheck.transform.position + transform.right * wallCheckDistance);
    }

    private Rect Offset(Rect wallCheck)
    {
        var wallCheckOffset = wallCheck;
        wallCheckOffset.x *= Math.Sign(transform.right.x);
        wallCheckOffset.width *= Math.Sign(transform.right.x);
        wallCheckOffset.position += (Vector2)transform.position;
        return wallCheckOffset;
    }

    public override bool CanMoveToNow(Vector3 position)
    {
        return !InFrontEnemuesCheck();
    }
}