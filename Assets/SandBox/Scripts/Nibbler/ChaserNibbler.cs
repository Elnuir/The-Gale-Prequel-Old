using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChaserNibbler : MonoBehaviour
{
    public WaypointContainer wayPoints;
    //public Transform wayPoint2;
   // public GameObject triggerChaseStarter;
    private EnemyAIPathAndMoveNibbler movement;
    private WaypointMovement waypointMovement;
    public bool isChasingPlayer;
    public float stopDistanceToAPoint;
    private Nibbler nibbler;
    
    
    void Start()
    {
        movement = GetComponentInParent<EnemyAIPathAndMoveNibbler>();
        waypointMovement = GetComponentInParent<WaypointMovement>();
        nibbler = GetComponentInParent<Nibbler>();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
//        Debug.Log(other.gameObject.name);
        if (other.CompareTag("Player"))
        {    
                movement.target = other.gameObject.transform;
                isChasingPlayer = true;
                
//                print("StartChase");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log(other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            RandomPointPicker();
            isChasingPlayer = false;
            //nibbler.isIdling = false;
           // nibbler.isMoving = true;
            //print("DDDDD");
            //   print("StopChase");
        }
    }
    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.gameObject.CompareTag("Enemy"))
    //     {
    //         RandomPointPicker();
    //         print("hui" );
    //     }
    // }

    void RandomPointPicker()
    {
        var a = Random.Range(0, wayPoints.waypoints.Count);
        
        
        movement.target = wayPoints.waypoints[a];
        
        
    }
}
