using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfinerCollideCheck : MonoBehaviour
{
    [SerializeField]private ChaserBlackMatter chaser;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 24 && chaser.isImpacting)
        {
            chaser.isImpacting = false;
        }
    }
}
