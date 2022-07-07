using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFaseThreeMover : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.right*speed*Time.fixedDeltaTime;
    }
}
