using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    public float Force;
    public float Time;

    private bool upForce;
    private Rigidbody2D rb;

    private Vector2 currVelocity;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (ActionEx.CheckCooldown(LateUpdate, Time))
            upForce = !upForce;

        var force = new Vector2(rb.velocity.x, upForce ? Force : -Force);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, force, ref currVelocity, 0.4f);
    }
}
