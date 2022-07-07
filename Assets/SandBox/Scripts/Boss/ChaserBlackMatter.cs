using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChaserBlackMatter : MonoBehaviour
{
    public WaypointContainer wayPoints;

    public int currentWayPoint;

    //public Transform wayPoint2;
    // public GameObject triggerChaseStarter;
    private EnemyAIPathAndMoveBlackMatter movement;
    private BlackMatter blackMatter;
    private WaypointMovement waypointMovement;
    public bool isChasingPlayer;

    public float stopDistanceToAPoint, timeOfImpactingLeft, timeToStayOnSafeSpotLeft;

    // [SerializeField] private float timeToStartChase;
    [SerializeField] private float timeToStayOnSpot, timeToStayOnSafeSpotBase, timeOfImpactingBase, impactSpeed;
    [SerializeField] Transform[] safeSpots;
    public bool isOnSpot, isOnSafeSpot;
    private bool isInvoked;
    public bool isImpacting;

    private Player player;

    private Rigidbody2D rb;

    private float distanceToThePoint;

    private Vector2 reference;

    [SerializeField] Animator animator;
    // private float timeToStartChaseLeft;
    // private Boss boss;


    void Start()
    {
        timeOfImpactingLeft = timeOfImpactingBase;
        timeToStayOnSafeSpotLeft = timeToStayOnSafeSpotBase;
        rb = GetComponentInParent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        currentWayPoint = 0;
        // timeToStartChaseLeft = timeToStartChase;
        //boss = GetComponentInParent<Boss>();
        movement = GetComponentInParent<EnemyAIPathAndMoveBlackMatter>();
        blackMatter = GetComponentInParent<BlackMatter>();
        waypointMovement = GetComponentInParent<WaypointMovement>();
        RandomPointPicker();
    }

    // Update is called once per frame
    void Update()
    {
        BlackMatterStates();
        if (!isChasingPlayer)
        {
            animator.SetBool("isImpacting", isImpacting);
           // ImpactCounter();
            OnSpotStopper();
            OnSafeSpotStopper();
            SafeSpotCounter();
//            print("shit");
            distanceToThePoint = Vector2.Distance(gameObject.transform.position, movement.target.transform.position);
            if (distanceToThePoint < stopDistanceToAPoint && !isImpacting && movement.target.GetComponent<dotClamping>() == null)
            {
                isOnSafeSpot = true;
                print("SAFESPOT");
            }
            
            else if (distanceToThePoint < stopDistanceToAPoint && !isImpacting && movement.target.GetComponent<dotClamping>() != null && !blackMatter.isFaseFour)
            {
                print("smth4");
                isOnSpot = true;
                if (movement.target.transform.position.x < player.transform.position.x && movement.target.GetComponent<dotClamping>().isClampedX)
                {
                    if (!isInvoked)
                    {
                        Invoke(nameof(AttackRight), timeToStayOnSpot);
                        isInvoked = true;
                        isImpacting = true;
                        print("smth");
                    }
                }
                else if (movement.target.transform.position.x > player.transform.position.x && movement.target.GetComponent<dotClamping>().isClampedX)
                {
                    if (!isInvoked)
                    {
                        Invoke(nameof(AttackLeft), timeToStayOnSpot);
                        isInvoked = true;
                        isImpacting = true;
                        print("smth2");
                    }
                }
                else if (movement.target.transform.position.y > player.transform.position.y && movement.target.GetComponent<dotClamping>().isClampedY)
                {
                    if (!isInvoked)
                    {
                        Invoke(nameof(AttackDown), timeToStayOnSpot);
                        isInvoked = true;
                        isImpacting = true;
                        print("smth3");
                    }
                }
                // else if(distanceToThePoint < stopDistanceToAPoint && !isImpacting && movement.target.GetComponent<dotClamping>() == null)
                // {
                //     if (!isInvoked)
                //     {
                //         isOnSafeSpot = true;
                //         Invoke(nameof(RandomPointPicker), timeToStayOnSafeSpot);
                //         isInvoked = true;
                //     }
                // }

                //if (!isInvoked)
                // {
                //    Invoke(nameof(RandomPointPicker), timeToStayOnSpot);
                //    isInvoked = true;
                // }

                //RandomPointPicker();
            }
        }
        
    }

    void BlackMatterStates()
    {
        animator.SetBool("hit", blackMatter.isHit);
    }

    void OnSpotStopper()
    {
        if (isOnSpot) //|| isOnSafeSpot)
        {
            rb.velocity = Vector2.SmoothDamp(rb.velocity, Vector2.zero, ref reference, 0.4f);
        }
    }
    void OnSafeSpotStopper()
    {
        if (isOnSafeSpot && !isImpacting) //|| isOnSafeSpot)
        {
            rb.velocity = Vector2.SmoothDamp(rb.velocity, Vector2.zero, ref reference, 0.4f);
        }
    }

    // void ImpactCounter()
    // {
    //     if (isImpacting)
    //     {
    //         timeOfImpactingLeft -= Time.deltaTime;
    //         if (timeOfImpactingLeft <= 0)
    //         {
    //             isImpacting = false;
    //             timeOfImpactingLeft = timeOfImpactingBase;
    //             
    //         }
    //     }
    // }
    void SafeSpotCounter()
    {
        if (isOnSafeSpot)
        {
            timeToStayOnSafeSpotLeft -= Time.deltaTime;
            if (timeToStayOnSafeSpotLeft <= 0)
            {
                isOnSafeSpot = false;
                timeToStayOnSafeSpotLeft = timeToStayOnSafeSpotBase;
                RandomPointPicker();
            }
        }
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     print(other.gameObject.layer);
    //     if (other.gameObject.layer == 24 && isImpacting)
    //     {
    //         isImpacting = false;
    //         print(other.gameObject.layer + "AAAAARRRRR");
    //     }
    // }

    void AttackRight()
    {
        print("Right");
        isOnSpot = false;
        isInvoked = false;
        if (isImpacting)
            rb.velocity = new Vector2(1, 0) * impactSpeed; //* Time.deltaTime;
        SafeSpotPicker();
        //RandomPointPicker();
    }

    void AttackLeft()
    {
        isOnSpot = false;
        isInvoked = false;
        if (isImpacting)
            rb.velocity = new Vector2(-1, 0) * impactSpeed; //* Time.deltaTime;
        print("Left");
        SafeSpotPicker();
       // RandomPointPicker();
    }

    void AttackDown()
    {
        isOnSpot = false;
        isInvoked = false;
        if (isImpacting)
            rb.velocity = new Vector2(0, -1) * impactSpeed; //* Time.deltaTime;
        SafeSpotPicker();
       // RandomPointPicker();
        print("DowN");
    }
// private void OnTriggerStay2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         timeToStartChaseLeft -= Time.deltaTime;
    //         if (timeToStartChaseLeft <= 0.0f)
    //         {
    //             movement.target = other.gameObject.transform;
    //             isChasingPlayer = true;
    //         }
    //     }
    //
    //    
    // }
    //
    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         RandomPointPicker();
    //         isChasingPlayer = false;
    //         timeToStartChaseLeft = timeToStartChase;
    //     }
    // }

    void SafeSpotPicker()
    {
        isInvoked = false;
        isOnSpot = false;
        if(!blackMatter.isFaseFour)
        movement.target = safeSpots[Random.Range(0, safeSpots.Length)];
    }
    void RandomPointPicker()
    {
        isInvoked = false;
        isOnSpot = false;
        isOnSafeSpot = false;
        if (currentWayPoint <= wayPoints.waypoints.Count && !blackMatter.isFaseFour)
        {
            movement.target = wayPoints.waypoints[currentWayPoint++];
        }
        else if (currentWayPoint > wayPoints.waypoints.Count && !blackMatter.isFaseFour)
        {
            currentWayPoint = 0;
            movement.target = wayPoints.waypoints[currentWayPoint];
        }
    }
}