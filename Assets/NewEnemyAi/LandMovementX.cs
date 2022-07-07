using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementStats))]
[RequireComponent(typeof(Rigidbody2D))]
public class LandMovementX : PathMovementBaseX
{
    public float Speed = 5f;
    public float JumppForce = 50f;
    public float JumpCooldown = 0.5f;
    public float ChangeDirectionCooldown = 1f;
    public bool InitFacingRight;

    [Range(0f, 1f)] public float Smoothness = 0.1f;
    [Range(0f, 1f)] public float AirControl = 0f;

    public Rect WallCheck;

    private MovementStats _movementStats;
    private Rigidbody2D _physics;
    private Vector2 _currentVelocity;
    private bool IsFacingRight => InitFacingRight && transform.right.x > 0 || !InitFacingRight && transform.right.x < 0;

    protected override void Start()
    {
        base.Start();
        _movementStats = GetComponent<MovementStats>();
        _physics = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        base.Update();
        FlipFix();
    }

    public override void MoveTo(Vector3 position)
    {
        Destination = position;
        Vector3 node = CurrentNode ?? position;

        float speed = _movementStats.IsGrounded ? Speed : Speed * AirControl;
        Vector2 resultVelocity = Vector2.right * Mathf.Sign(node.x - transform.position.x) * Speed;

        if (Smoothness > float.Epsilon)
            resultVelocity = Vector2.SmoothDamp(_physics.velocity, resultVelocity, ref _currentVelocity, Smoothness);

        if (!IsDirectionChanged(resultVelocity) || CheckChangeDirectionCooldown())
            _physics.velocity = new Vector2(resultVelocity.x, _physics.velocity.y);

        if (ShouldJump())
            _physics.AddForce(Vector2.up * JumppForce, ForceMode2D.Impulse);
    }


    public bool ShouldJump()
    {
        if (!_movementStats.IsGrounded) return false;

        var wallCheckOffset = Offset(WallCheck);

        Collider2D wallCheck = Physics2D.OverlapArea(wallCheckOffset.min, wallCheckOffset.max,
            _movementStats.WhatIsGround);

        if (wallCheck != default) return ((Func<bool>)ShouldJump).CheckCooldown(JumpCooldown);

        return false;
    }

    private void FlipFix()
    {
        if (_physics.velocity.x > 1e-5f && !IsFacingRight)
            transform.Rotate(new Vector3(0, 180, 0));

        if (_physics.velocity.x < -1e-5f && IsFacingRight)
            transform.Rotate(new Vector3(0, 180, 0));
    }

    private Rect Offset(Rect wallCheck)
    {
        var wallCheckOffset = wallCheck;
        wallCheckOffset.x *= Math.Sign(transform.right.x);
        wallCheckOffset.width *= Math.Sign(transform.right.x);
        wallCheckOffset.position += (Vector2)transform.position;
        return wallCheckOffset;
    }

    private void OnDrawGizmos()
    {
        var wallCheckOffset = Offset(WallCheck);

        Collider2D wallCheck = Physics2D.OverlapArea(wallCheckOffset.min, wallCheckOffset.max,
            GetComponent<MovementStats>().WhatIsGround);

        if (wallCheck)
            Gizmos.color = Color.red;

        Gizmos.DrawWireCube(wallCheckOffset.center, wallCheckOffset.size); //wallCheckA.transform.position, wallCheck.transform.position + transform.right * wallCheckDistance);
    }


    private bool CheckChangeDirectionCooldown()
        => ((Func<bool>)CheckChangeDirectionCooldown).CheckCooldown(ChangeDirectionCooldown);
    private bool IsDirectionChanged(Vector2 newVelocity)
        => newVelocity.x > 0 && !IsFacingRight || newVelocity.x < 0 && IsFacingRight;
    public override void StopMovement()
        => _physics.velocity = new Vector2(0, _physics.velocity.y);
}
