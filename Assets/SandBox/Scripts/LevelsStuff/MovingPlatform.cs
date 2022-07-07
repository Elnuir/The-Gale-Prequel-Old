using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 movePosition;
    [SerializeField] private float moveSpeed;

    [SerializeField] [Range(0,1)] float moveProgress;

    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        moveProgress = Mathf.PingPong(Time.time*moveSpeed, 1);
        Vector3 offset = movePosition * moveProgress;
        transform.position = startPosition + offset;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // print(other);
        // if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        // {
        //     other.collider.transform.SetParent(transform);
        // }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        // {
        //     other.collider.transform.SetParent(null);
        // }
    }
}
