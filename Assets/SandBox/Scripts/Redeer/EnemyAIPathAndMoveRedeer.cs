using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAIPathAndMoveRedeer : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Redeer redeer;
    [SerializeField] private float distanceToStopWhenAttack;
    [SerializeField] private float distanceToBackOffWhenAttack;

    private ChaserRedeer chaser;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        redeer = GetComponent<Redeer>();
        chaser = GetComponentInChildren<ChaserRedeer>();

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
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 noSeekerDirection = (target.transform.position - transform.position).normalized;
        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;
        if (redeer.isJustSpawned)
        {
            rb.AddForce(Vector2.down*15f);
        }
        else if (!redeer.isDead && !redeer.isChargingAttack && !redeer.isAttacking)
        {
            redeer.isMoving = true;
            if (!chaser.isChasingPlayer)
                rb.AddForce(force);
            //if (!chaser.isChasingPlayer)
            // rb.velocity = force;
            else if (chaser.isChasingPlayer)
            {
                if (Vector2.Distance(target.transform.position, gameObject.transform.position) <= distanceToBackOffWhenAttack)
                {
                    rb.AddForce(-force);
                }

                else if (Vector2.Distance(target.transform.position, gameObject.transform.position) <= distanceToStopWhenAttack)
                {
                    rb.AddForce(new Vector2(0, noSeekerDirection.y * speed * Time.fixedDeltaTime));
                }
                else// if (Vector2.Distance(target.transform.position, gameObject.transform.position) > 8)
                {
                    rb.AddForce(force);
                }
            }

//            print(Vector2.Distance(target.transform.position, gameObject.transform.position));
            // else if (chaser.isChasingPlayer)
            //     rb.AddForce(-force);
        }
        else
        {
            redeer.isMoving = false;
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}