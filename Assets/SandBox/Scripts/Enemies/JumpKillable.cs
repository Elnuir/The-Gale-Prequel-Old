using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpKillable : MonoBehaviour
{
    public bool isJumpKillable;

    private void OnCollisionEnter2D(Collision2D other)
    {
//        print(other.gameObject);
    }
}
