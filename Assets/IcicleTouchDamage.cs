using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleTouchDamage : MonoBehaviour
{
    public string PlayerTag = "Player";
    public float Damage = 20;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(PlayerTag))
        {
            float[] attackDetails = new[]
            {
                Damage,
                transform.position.x
            };
            other.gameObject.SendMessage("Damage", attackDetails);
        }
    }
}