using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformHook : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy")) //&& other.gameObject.GetComponent<PlayerClimb>().isHooked)
        {
            other.gameObject.transform.SetParent(transform.parent);
            print(other);
        }
        // if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        // {
        //     other.transform.SetParent(transform);
        // }
        // if (other.gameObject.CompareTag("Projectile"))
        // {
        // other.gameObject.transform.SetParent(transform.parent);
        //}

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //if (other.gameObject.CompareTag("Player") && !other.gameObject.GetComponent<PlayerClimb>().isHooked)
        // {
        //    other.gameObject.transform.SetParent(null);
        //  }
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.transform.SetParent(null);
        }
    }
}
