using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ShootingSpotJumpCrutch : MonoBehaviour
{
    public Player player;
    private bool StayStill;
    private float StayStillY;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isGrounded && !StayStill)
        {
            StayStill = true;
            StayStillY = transform.position.y;
        }

        if (player.isGrounded && StayStill)
        {
            StayStill = false;
            transform.position = new UnityEngine.Vector3(transform.position.x, player.transform.position.y);
        }

        if (StayStill)
        {
            transform.position = new UnityEngine.Vector3(transform.position.x, StayStillY);
        }

    }
}
