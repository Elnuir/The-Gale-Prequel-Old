using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stash : MonoBehaviour
{
    private DashMove dashMove;

    private Transform[] pieces;
    //[SerializeField] private float rotationSpeed;

    private Rigidbody2D rbKid;
    private Animator animator;
    private PolygonCollider2D polygonCollider2D;
   // private GameObject stashIndicator;

    [SerializeField]float flyspeed;

    public bool crashable;
    // Start is called before the first frame update
    void Start()
    {
       // stashIndicator = FindObjectOfType<StashManager>().stashIndicator;
      //  pieces = GetComponentsInChildren<Transform>();
      pieces = GetComponentsInChildren<Transform>().Where(p => p.transform != transform).ToArray();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (crashable)
        {
            if (other.gameObject.layer == 20)
            {
                dashMove = FindObjectOfType<DashMove>();
                if (dashMove.isDashing)
                {
                    foreach (var piece in pieces)
                    {
                        rbKid = piece.gameObject.GetComponent<Rigidbody2D>();
                        animator = piece.gameObject.GetComponent<Animator>();
                        rbKid.velocity = GetRandomVector() * flyspeed * Time.deltaTime;
                        animator.enabled = true;
                        Invoke(nameof(DestroyAll), 1f);
                    }

                    polygonCollider2D.enabled = false;
                }
            }
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (crashable)
    //     {
    //         if (other.CompareTag("Player"))
    //             stashIndicator.SetActive(true);
    //     }
    // }
    //
    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (crashable)
    //     {
    //         if (other.CompareTag("Player"))
    //             stashIndicator.SetActive(false);
    //     }
    // }

    private Vector2 GetRandomVector()
    {
        int signX = Math.Sign(Random.Range(-10, 10));
        int signY = Math.Sign(Random.Range(-10, 10));

        if (signX == 0) signX = 1;
        if (signY == 0) signY = 1;

        var v =  new Vector2(signX * Random.Range(5, 10), signY * Random.Range(5, 10));
        return v;
    }

    void DestroyAll()
    {
        Destroy(gameObject);
    }
}
