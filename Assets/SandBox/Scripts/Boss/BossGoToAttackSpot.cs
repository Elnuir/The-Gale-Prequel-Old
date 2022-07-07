using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BossMovement))]
public class BossGoToAttackSpot : AnimatedEntityStateBase
{
    public float IsReachedDistance = 5;
    public float Interval = 20;
    public WaypointContainer AttackSpots;
    public LayerMask WhatIsGround;
    public float SpotNoGroundRadius = 3;
    public TargetProviderBase Target;
    [HideInInspector] public Vector2 CurrentSpot;


    private BossMovement _movement;
    private Rigidbody2D _physics;
    private float _timer;

    public AnimatedEntityState[] MakeAvailableOnSpotReached;

    public override bool IsAvailable
    {
        get
        {
            if (CurrentSpot != default)
                return _timer >= Interval && Vector2.Distance(transform.position, CurrentSpot) >=
                    IsReachedDistance * 0.9f;
            return _timer >= Interval;
        }
    }

    protected override void Start()
    {
        base.Start();
        _timer = Interval - 1;
        _movement = GetComponent<BossMovement>();
        _physics = GetComponent<Rigidbody2D>();
    }

    public override void ActivateState()
    {
        base.ActivateState();
        if (ActionEx.CheckCooldown(ActivateState, 3f))
            SelectCurrentSpot();
        Animator.Play(AnimationName);
    }


    public override void DeactivateState()
    {
        base.DeactivateState();
        _physics.velocity = Vector2.zero;
        //   _timer = 0;

        if (Vector2.Distance(transform.position, CurrentSpot) <= IsReachedDistance)
        {
            Array.ForEach(MakeAvailableOnSpotReached, s => s.MakeAvailable());
            _timer = 0;
        }
    }

    protected void Update()
    {
        if (IsActive)
            _movement.MoveTo(CurrentSpot);
        else
            _timer += Time.deltaTime;
        Debug.Log(_timer);
    }

    private void SelectCurrentSpot()
    {
        var goodSpots = AttackSpots.waypoints.Where(CheckWaypoint).ToArray();
        var bestSpots = goodSpots.Where(CanHitPlayerFrom).ToArray();

        if (bestSpots.Length > 0)
            CurrentSpot = bestSpots[Random.Range(0, bestSpots.Length)].position;
        else if (goodSpots.Length > 0)
            CurrentSpot = goodSpots[Random.Range(0, goodSpots.Length)].position;
        else
            CurrentSpot = AttackSpots.waypoints[Random.Range(0, AttackSpots.waypoints.Count)].position;
    }

    private bool CheckWaypoint(Transform wp)
    {
        var overlap = Physics2D.OverlapCircle(wp.position, SpotNoGroundRadius, WhatIsGround);
        return overlap == default;
    }

    private bool CanHitPlayerFrom(Transform wp)
    {
        float distance = Vector2.Distance(Target.transform.position, wp.position);
        Vector2 direction = (Target.transform.position - wp.position).normalized;
        var raycast = Physics2D.Raycast(wp.position, direction, distance, WhatIsGround);

        return raycast == default;
    }
}