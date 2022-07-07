using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBuffCollectable : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HubWeaponBuffOpenCheck.OpenFireBuff(1);
            Destroy(gameObject);
        }
    }
}
