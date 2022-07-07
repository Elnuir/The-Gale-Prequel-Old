using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItdimkPatroler : MonoBehaviour
{
    private ItdimkEnemyStateManager _stateManager;
    public WaypointContainer Waypoints;
    public LayerMask WhatIsGround;
    [SerializeField] Rect WallCheck;

    private int currWpIndex;
    private Rigidbody2D _physics;
    private MovementStats _movementStats;

    // Start is called before the first frame update
    void Start()
    {
        _stateManager = GetComponent<ItdimkEnemyStateManager>();
        _physics = GetComponent<Rigidbody2D>();
        _movementStats = GetComponent<MovementStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_stateManager.CurrentState == ItdimkEnemyStateManager.NibblerState.Patrol)
        {
            GoNextWaypoint();
            FlipFix();
        }
    }

   public  bool ShouldJump()
    {
        if (!((Func<bool>)ShouldJump).CheckCooldown(1f)) return false;
        if (!_movementStats.IsGrounded) return false;
        
        var wallCheckOffset = this.WallCheck;
        wallCheckOffset.x *= Math.Sign(transform.right.x);

        wallCheckOffset.position += (Vector2) transform.position;
        Collider2D wallCheck = Physics2D.OverlapArea(wallCheckOffset.position, wallCheckOffset.max,
                WhatIsGround);

        return wallCheck != default;
    }

    void GoNextWaypoint()
    {
        if(IsWaypointReached())
            PickNextWaypoint();
        
        var wp = Waypoints.waypoints[currWpIndex].transform.position;
        var currVelocity = _physics.velocity;


        if (wp.x > transform.position.x)
            _physics.velocity = new Vector2(2, currVelocity.y);
        else
            _physics.velocity = new Vector2(-2, currVelocity.y);
        
        if(ShouldJump())
            _physics.AddForce(new Vector2(0, 25), ForceMode2D.Impulse);
    }

    void PickNextWaypoint()
    {
        currWpIndex = Random.Range(0, Waypoints.waypoints.Count);
    }

    bool IsWaypointReached()
    {
        return Vector2.Distance(transform.position, Waypoints.waypoints[currWpIndex].transform.position) < 1f;
    }

    void FlipFix()
    {
        if (_physics.velocity.x > 0 != transform.right.x < 0)
            transform.Rotate(0, 180, 0);
    }

    private void OnDrawGizmos()
    {
        var wallCheckOffset = this.WallCheck;
        wallCheckOffset.x *= Math.Sign(transform.right.x);
        wallCheckOffset.position += (Vector2) transform.position;
        Gizmos.DrawWireCube(wallCheckOffset.center,
            wallCheckOffset
                .size); //wallCheckA.transform.position, wallCheck.transform.position + transform.right * wallCheckDistance);
    }
}