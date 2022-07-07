using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAIPathAndMoveNibbler : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float jumpForce = 200f;
    public float nextWaypointDistance = 3f;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Nibbler nibbler;
    private ChaserNibbler chaser;

    [SerializeField] Rect wallCheck;
    [SerializeField] private GameObject jumpCheck; //groundCheck; //ceilingCheck,;
    [SerializeField] private float jumpCheckDistance; // groundCheckDistance;// ceilingCheckDistance,;

    [SerializeField] private LayerMask whatIsGround;

    //public RaycastHit2D isGroundedCheckRay;
    public Collider2D groundCheck;
    [SerializeField] private Transform leftUpGroundCheck, rightDownGroundCheck;

    private Vector2 jumpUp;
    [SerializeField] private float heightToIdle;

    [SerializeField] private float jumpCooldown;

    // Start is called before the first frame update
    void Start()
    {
        chaser = GetComponentInChildren<ChaserNibbler>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        // target = FindObjectOfType<Player>().transform;
        nibbler = GetComponent<Nibbler>();

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

    private void Update()
    {
//        print(rb.velocity.x);
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

        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;

        //Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        //Vector2 force = direction * speed * Time.deltaTime;
        //rb.AddForce(force);       

        if (!nibbler.isDead && !nibbler.isChargingAttack && !nibbler.isAttacking && !nibbler.isHit &&
            chaser.isChasingPlayer )// && target.position.y - transform.position.y <= heightToIdle)
        {
            nibbler.isMoving = true;
            nibbler.isIdling = false;
            // print("AAAAAA");
            
            if(!GetComponent<DescendingFix>().IsGoingDown)
            rb.velocity = new Vector2(Mathf.Sign(force.x) * speed, rb.velocity.y);
            
            // rb.AddForce(force);//IMPORTANT
        }
        else if (!nibbler.isDead && !nibbler.isChargingAttack && !nibbler.isAttacking && nibbler.isHit &&
                 chaser.isChasingPlayer && target.position.y - transform.position.y > heightToIdle) //TODO: this
        {
            nibbler.isMoving = false;
            nibbler.isIdling = true;
            //rb.velocity = Vector2.zero;
            //  print("BBBBBB");
            if(!GetComponent<DescendingFix>().IsGoingDown)

            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else if (!nibbler.isDead && !nibbler.isChargingAttack && !nibbler.isAttacking && !nibbler.isHit &&
                 !chaser.isChasingPlayer )
        {
            nibbler.isMoving = true;
            nibbler.isIdling = false;
            if(!GetComponent<DescendingFix>().IsGoingDown)

            rb.velocity = new Vector2(Mathf.Sign(force.x) * speed, rb.velocity.y);
        }

        if ((nibbler.isChargingAttack || nibbler.isAttacking || nibbler.hit) && !nibbler.isHit)
        {
            nibbler.isMoving = false;
            // print("CCCCC");
            if(!GetComponent<DescendingFix>().IsGoingDown)

            rb.velocity = new Vector2(0, rb.velocity.y);
        }


        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        var wallCheckOffset = this.wallCheck;
        wallCheckOffset.x *= Math.Sign(transform.right.x);

        wallCheckOffset.position += (Vector2) transform.position;
        Collider2D wallCheck2 =
            Physics2D.OverlapArea(wallCheckOffset.position, wallCheckOffset.max,
                whatIsGround); //llCheckA.transform.position, wallCheckB.transform.position, whatIsGround);
        RaycastHit2D jumpCheckRay = Physics2D.Raycast(jumpCheck.transform.position, -jumpCheck.transform.right,
            jumpCheckDistance, whatIsGround);
        //isGroundedCheckRay = Physics2D.Raycast(groundCheck.transform.position, -groundCheck.transform.up, groundCheckDistance, whatIsGround);
        groundCheck = Physics2D.OverlapArea(leftUpGroundCheck.position, rightDownGroundCheck.position, whatIsGround);

        if ((wallCheck2 && groundCheck || !jumpCheckRay && groundCheck) && !nibbler.isAttacking &&
            !nibbler.isChargingAttack && !nibbler.isIdling)
        {
            Jump();
        }

        print((bool) jumpCheckRay);
    }

    void Jump()
    {
        if (!ActionEx.CheckCooldown(Jump, jumpCooldown)) return; //Reloading jump
        jumpUp = Vector2.up * jumpForce * Time.deltaTime;
        rb.AddForce(jumpUp);
//        print("Jump");
    }

    private void OnDrawGizmos()
    {
        //   var// rect =  new Rect(wallCheckA.transform.position.x, wallCheckA.transform.position.y, wallCheckB.transform.position.x - wallCheckA.transform.position.x, wallCheckB.transform.position.y - wallCheck);

        var wallCheckOffset = this.wallCheck;
        wallCheckOffset.x *= Math.Sign(transform.right.x);
        wallCheckOffset.position += (Vector2) transform.position;
        Gizmos.DrawWireCube(wallCheckOffset.center,
            wallCheckOffset
                .size); //wallCheckA.transform.position, wallCheck.transform.position + transform.right * wallCheckDistance);
        Gizmos.DrawLine(jumpCheck.transform.position,
            jumpCheck.transform.position - transform.right * jumpCheckDistance);
        //Gizmos.DrawLine(ceilingCheck.transform.position, ceilingCheck.transform.position + transform.up * ceilingCheckDistance);
        //Gizmos.DrawLine(groundCheck.transform.position, groundCheck.transform.position  - transform.up * groundCheckDistance);
        Gizmos.DrawWireCube(
            new Vector2((leftUpGroundCheck.position.x + rightDownGroundCheck.position.x) / 2,
                (leftUpGroundCheck.position.y + rightDownGroundCheck.position.y) / 2),
            new Vector2((rightDownGroundCheck.position.x - leftUpGroundCheck.position.x),
                (leftUpGroundCheck.position.y - rightDownGroundCheck.position.y)));
    }
}