using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlipProvider : MonoBehaviour
{
    public Transform Target;
    public bool isFacingRight;

    private void Update()
    {
        if (Target.position.x > transform.position.x && !isFacingRight)
        {
            isFacingRight = true;
            transform.Rotate(0, 180, 0);
        }

        if (Target.position.x < transform.position.x && isFacingRight)
        {
            isFacingRight = false; 
            transform.Rotate(0, 180, 0);
        }
    }
}
