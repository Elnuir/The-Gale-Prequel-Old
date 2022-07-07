using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFix : MonoBehaviour
{
    private Rigidbody2D _physics;
    public Vector2 Force;


    public MonoBehaviour[] ItemsToDeactivate;
    public float DeactivationTime;


    // Start is called before the first frame update
    private void OnEnable()
    {
        _physics = GetComponent<Rigidbody2D>();
        _physics.AddForce(Force, ForceMode2D.Impulse);

        Array.ForEach(ItemsToDeactivate, i => i.enabled = false);
        Invoke(nameof(EnableAll), DeactivationTime);
    }

    void EnableAll()
    {

        Array.ForEach(ItemsToDeactivate, i => i.enabled = true);
    }
}
