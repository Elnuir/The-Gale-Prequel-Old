using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[RequireComponent(typeof(Rigidbody2D))]
public class BossVelocityTargetProvider : TargetProviderBase
{
    private GameObject marker;
    private Rigidbody2D physics;
    public float ResetRotationDelay = 0.4f;
    private float resetRotationTimer;
    
    private void Start()
    {
        marker = new GameObject();
        physics = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        if (resetRotationTimer > 0)
            resetRotationTimer -= Time.deltaTime;
    }

    public override Transform GetTarget()
    {
        if (physics.velocity.magnitude > 1)
        {
            marker.transform.position = (Vector2) transform.position + physics.velocity * 4;
            resetRotationTimer = ResetRotationDelay;
        }
        else if(resetRotationTimer <= 0)
        {
            marker.transform.position = (Vector2) transform.position + new Vector2(-1, -1);
        }

        return marker.transform;
    }
}