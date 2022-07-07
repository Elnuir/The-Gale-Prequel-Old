using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChaserRedeer : MonoBehaviour
{
    public WaypointContainer wayPoints;
    //public Transform wayPoint2;
   // public GameObject triggerChaseStarter;
    private EnemyAIPathAndMoveRedeer movement;
    private WaypointMovement waypointMovement;
    public bool isChasingPlayer;
    public float stopDistanceToAPoint;
    private Redeer redeer;
    
    
    void Start()
    {
        redeer = GetComponentInParent<Redeer>();
        movement = GetComponentInParent<EnemyAIPathAndMoveRedeer>();
        waypointMovement = GetComponentInParent<WaypointMovement>();
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
                RandomPointPicker();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
//        Debug.Log(other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            movement.target = other.gameObject.transform;
            isChasingPlayer = true;
            if (transform.position.x - movement.target.transform.position.x > 0 && redeer.facingRight)
            {
                redeer.Flip();
            }
            else if (transform.position.x - movement.target.transform.position.x < 0 && !redeer.facingRight)
            {
                redeer.Flip();
            }
        }
        

       
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log(other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            RandomPointPicker();
            isChasingPlayer = false;
//            print("StopChase");
        }
    }

    

    void RandomPointPicker()
    {
        var a = Random.Range(0, wayPoints.waypoints.Count);
        movement.target = wayPoints.waypoints[a];
    }
}
