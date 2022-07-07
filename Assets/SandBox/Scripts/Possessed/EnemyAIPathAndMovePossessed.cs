using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAIPathAndMovePossessed : MonoBehaviour
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
    private Possessed possessed;
    private ChaserPossessed chaser;

    [SerializeField] Rect wallCheck;
    [SerializeField] private GameObject jumpCheck, farWallCheck; //groundCheck; //ceilingCheck,;
    [SerializeField] private float jumpCheckDistance, farWallDistance; //groundCheckDistance;// ceilingCheckDistance,;
    [SerializeField] private LayerMask whatIsGround;
  //  public RaycastHit2D isGroundedCheckRay;
  public Collider2D groundCheck;
    [SerializeField] private Transform leftUpGroundCheck, rightDownGroundCheck;
    
    private Vector2 jumpUp;
    [SerializeField] private float heightToIdle;

    [SerializeField] private float jumpCooldown;
    
    private RaycastHit2D farWallHit;
    // Start is called before the first frame update
    void Start()
    {
        chaser = GetComponentInChildren<ChaserPossessed>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
       // target = FindObjectOfType<Player>().transform;
        possessed = GetComponent<Possessed>();
        
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

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;
        //rb.AddForce(force);       
        if (!possessed.isDead && !possessed.isChargingAttack && !possessed.isAttacking && chaser.isChasingPlayer && target.position.y - transform.position.y <= heightToIdle)
        {
            possessed.isMoving = true;
            possessed.isIdling = false;
            // print("AAAAAA");
            rb.velocity = new Vector2(Mathf.Sign(force.x) * speed, rb.velocity.y);
            // rb.AddForce(force);//IMPORTANT
        }
        else if (!possessed.isDead && !possessed.isChargingAttack && !possessed.isAttacking && chaser.isChasingPlayer && target.position.y - transform.position.y > heightToIdle && !farWallHit) //TODO: this
        {
            possessed.isMoving = false;
            possessed.isIdling = true;
            //rb.velocity = Vector2.zero;
            //  print("BBBBBB");
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else if (!possessed.isDead && !possessed.isChargingAttack && !possessed.isAttacking && !chaser.isChasingPlayer)
        {
            possessed.isMoving = true;
            possessed.isIdling = false;
            rb.velocity = new Vector2(Mathf.Sign(force.x) * speed, rb.velocity.y);
        }
        else if ((possessed.isChargingAttack||possessed.isAttacking) && !possessed.isDead)
        {
            possessed.isMoving = false;
            // print("CCCCC");
            print("YYY");
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
         // if (!possessed.isDead && !possessed.isChargingAttack &&!possessed.isAttacking)
         // {
         //     possessed.isMoving = true;
         //     rb.velocity = new Vector2(force.x, rb.velocity.y); //IMPORTANT
         // }
         // else
         // {
         //     possessed.isMoving = false;
         //     rb.velocity = Vector2.zero;
         // }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        var wallCheckOffset = this.wallCheck;
        wallCheckOffset.x *= Math.Sign(transform.right.x);

        wallCheckOffset.position += (Vector2) transform.position;
        Collider2D wallCheck2 = Physics2D.OverlapArea(wallCheckOffset.position, wallCheckOffset.max, whatIsGround); //llCheckA.transform.position, wallCheckB.transform.position, whatIsGround);
        RaycastHit2D jumpCheckRay = Physics2D.Raycast(jumpCheck.transform.position, -jumpCheck.transform.right, jumpCheckDistance, whatIsGround);
        farWallHit = Physics2D.Raycast(farWallCheck.transform.position, -farWallCheck.transform.right, farWallDistance, whatIsGround);
        //isGroundedCheckRay = Physics2D.Raycast(groundCheck.transform.position, -groundCheck.transform.up, groundCheckDistance, whatIsGround);
        groundCheck = Physics2D.OverlapArea(leftUpGroundCheck.position, rightDownGroundCheck.position, whatIsGround);

       // if (wallCheck2 || !jumpCheckRay && isGroundedCheckRay)
        if ((wallCheck2 && groundCheck || !jumpCheckRay && groundCheck) && !possessed.isAttacking && !possessed.isChargingAttack && !possessed.isIdling)
        {
            Jump();
        }
        print((bool)jumpCheckRay);
        

    }

    void Jump()
    {
        if(!ActionEx.CheckCooldown(Jump,jumpCooldown)) return; //Reloading jump
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
     Gizmos.DrawWireCube(wallCheckOffset.center, wallCheckOffset.size);//wallCheckA.transform.position, wallCheck.transform.position + transform.right * wallCheckDistance);
        Gizmos.DrawLine(jumpCheck.transform.position, jumpCheck.transform.position  - transform.right * jumpCheckDistance);
        Gizmos.DrawLine(farWallCheck.transform.position, farWallCheck.transform.position  - transform.right * farWallDistance);
        //Gizmos.DrawLine(ceilingCheck.transform.position, ceilingCheck.transform.position + transform.up * ceilingCheckDistance);
       // Gizmos.DrawLine(groundCheck.transform.position, groundCheck.transform.position  - transform.up * groundCheckDistance);
       Gizmos.DrawWireCube(new Vector2((leftUpGroundCheck.position.x + rightDownGroundCheck.position.x)/2, (leftUpGroundCheck.position.y + rightDownGroundCheck.position.y)/2),
           new Vector2((rightDownGroundCheck.position.x - leftUpGroundCheck.position.x), (leftUpGroundCheck.position.y - rightDownGroundCheck.position.y)));
    }
}
