using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Random = UnityEngine.Random;

public class EnemyAIPathAndMoveBlackMatter : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;
  //  [SerializeField] private Transform[] pointsToGo;
    private bool isflyattacking;
    private bool isgoingtoasafespot;
    [SerializeField] float attackTime, attackTimeLeft, safeTime, safeTimeLeft;
    private Vector2 direction;
    private Vector2 force;
    private Vector2 reference;
    private ChaserBlackMatter chaser;
    private BlackMatter blackMatter;

    // Start is called before the first frame update
    void Start()
    {
        chaser = GetComponentInChildren<ChaserBlackMatter>();
        attackTimeLeft = attackTime;
        safeTimeLeft = safeTime;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        blackMatter = GetComponent<BlackMatter>();
      //  target = pointsToGo[Random.Range(0, 2)];

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

        direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
        force = direction * speed * Time.fixedDeltaTime;
        if ((!chaser.isOnSpot && !chaser.isImpacting && !chaser.isOnSafeSpot) || blackMatter.isFaseFour)
        {
            rb.velocity = Vector2.SmoothDamp(rb.velocity, force, ref reference, 0.4f);
        }
        // else if(chaser.isOnSpot)
        // {
        //     rb.velocity = Vector2.zero;
        // }


        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

}