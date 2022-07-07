using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChaserBoss : MonoBehaviour
{
    public WaypointContainer wayPoints;
    public int currentWayPoint;
    //public Transform wayPoint2;
   // public GameObject triggerChaseStarter;
    private EnemyAIPathAndMoveBoss movement;
    private WaypointMovement waypointMovement;
    public bool isChasingPlayer;
    public float stopDistanceToAPoint;
   // [SerializeField] private float timeToStartChase;
    //private float timeToStartChaseLeft;
    [SerializeField] private float timeToStayOnSpot;
    public bool isOnSpot;
    private bool isInvoked;
    private Boss boss;
    [SerializeField] private Transform spotToGoToOnTheSecondFase;


    void Start()
    {
        currentWayPoint = 0;
        //timeToStartChaseLeft = timeToStartChase;
      //  boss = GetComponentInParent<Boss>();
        movement = GetComponentInParent<EnemyAIPathAndMoveBoss>();
        waypointMovement = GetComponentInParent<WaypointMovement>();
        boss = GetComponentInParent<Boss>();
        RandomPointPicker();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChasingPlayer)
        {
            float distanceToThePoint = Vector2.Distance(gameObject.transform.position, movement.target.transform.position);
            if (distanceToThePoint < stopDistanceToAPoint)
            {
                isOnSpot = true;
                if (!isInvoked  && !boss.isFaseTwo)
                {
                    Invoke(nameof(RandomPointPicker), timeToStayOnSpot);
                    isInvoked = true;
                }

                //RandomPointPicker();
            }
        }
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
    
    void RandomPointPicker()
    {
        isInvoked = false;
        isOnSpot = false;
        
        
        if (currentWayPoint <= wayPoints.waypoints.Count && !boss.isFaseTwo)
        {
            movement.target = wayPoints.waypoints[currentWayPoint++];
        }
        else if (currentWayPoint > wayPoints.waypoints.Count && !boss.isFaseTwo)
        {
            currentWayPoint = 0;
            movement.target = wayPoints.waypoints[currentWayPoint];
        }
    }
}
