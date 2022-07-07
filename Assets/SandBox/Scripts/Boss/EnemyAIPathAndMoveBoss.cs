using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAIPathAndMoveBoss : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Boss boss;
    public float BobbingForce;
    public float BobbingTime;

    private Vector2 currVelocityRef;
    private bool upBobbingForce;

    private ChaserBoss chaserBoss;

    // Start is called before the first frame update
    void Start()
    {
        chaserBoss = GetComponentInChildren<ChaserBoss>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        boss = GetComponent<Boss>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ActionEx.CheckCooldown(FixedUpdate, 2f))
            AstarPath.active.Scan(AstarPath.active.data.gridGraph);

        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            print("Yo");
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;

        if (!boss.isDead && !boss.isChargingAttack && !boss.isAttacking &&
            !chaserBoss.isOnSpot) //NOT MOVING WHEN ON SPOT
        {
            //boss.isMoving = true;
            if (!chaserBoss.isChasingPlayer)
                rb.velocity = force;
            else
                rb.velocity = -force;
        }
        else
        {
            //boss.isMoving = false;
            rb.velocity = Vector2.zero;
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (ShouldAddBobbing())
            AddBobbing();
    }


    bool ShouldAddBobbing()
    {
        return !boss.isChargingAttack && Mathf.Abs(rb.velocity.y) < 1f;
    }

    void AddBobbing()
    {
        if (ActionEx.CheckCooldown(AddBobbing, BobbingTime))
            upBobbingForce = !upBobbingForce;

        var force = new Vector2(rb.velocity.x, upBobbingForce ? BobbingForce : -BobbingForce);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, force, ref currVelocityRef, 0.4f);
    }
}