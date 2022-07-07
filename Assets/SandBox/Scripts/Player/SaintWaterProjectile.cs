using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaintWaterProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasHit;
    [SerializeField] private float rotationSpeed, exorcismRadius;
   public LayerMask StopMask, whatToExorcise;
   private Collider2D[] enemiesToExorcism;
    [SerializeField] private Transform exorcismCenter;
   // private float[] attackDetails = new float[2];
   // private float attackDamage;
    private Throw throw1;
   // private KnivesCounter knivesCounter;
    private bool triggered;
    [SerializeField] ParticleSystem squirt, shards;
    private bool squirtPlayed;
    void Start()
    {
       // attackDamage = FindObjectOfType<Throw>().attackDamage;
       // knivesCounter = FindObjectOfType<KnivesCounter>();
        throw1 = FindObjectOfType<Throw>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        // if (hasHit == false)
        // {
        //     transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        // }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!hasHit && StopMask == (StopMask | (1 << other.gameObject.layer)))
        {
            hasHit = true;
            CommitExorcism();
            if (!squirtPlayed)
            {
                shards.Play();
                squirt.Play();
                squirtPlayed = true;
            }

            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Destroy(gameObject, 2f);
        }
    }

    void CommitExorcism()
    {
        if (hasHit)
        {
            enemiesToExorcism = Physics2D.OverlapCircleAll(exorcismCenter.transform.position, exorcismRadius, whatToExorcise);
            foreach (var possessed in enemiesToExorcism)
            {
                if (possessed != null && possessed.gameObject != null)
                {
                    if (possessed.GetComponent<NPossessedHealth>()?.isBelowExorcism == true)
                    {
                        possessed.GetComponent<NPossessedHealth>().Dead(true);
                    }
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(exorcismCenter.transform.position, exorcismRadius);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (hasHit = true)
        // {
        //     Collider2D
        //     Possessed[] possesseds = FindObjectsOfType<Possessed>();
        //     foreach (var possessed in possesseds)
        //     {
        //         possessed.Dead();
        //     }
        // }
            // if (gameObject.layer == 15 && other.CompareTag("Player") && !triggered)
            // {
            //     triggered = true;
            //  //   throw1.amountOfKnives++;
            //  //throw1.amountOfKnives++;
            //  //knivesCounter.amount = throw1.amountOfKnives;
            //    // knivesCounter.TextUpdate();
            //     Destroy(gameObject);
            // }
            //
            // if (StopMask == (StopMask | (1 << other.gameObject.layer)))
            // {
            //     hasHit = true;
            //     rb.velocity = Vector2.zero;
            //     rb.isKinematic = true;
            //     gameObject.layer = 15; //Makes it expired
            // }
            //
            // if (other.CompareTag("Enemy") && !other.isTrigger)
            // {
            //     if (!hasHit)
            //     {
            //         //attackDetails[0] = attackDamage;
            //         //attackDetails[1] = transform.position.x;
            //         //other.transform.SendMessage("Damage", attackDetails);
            //         Debug.Log("Enemyhit");
            //     }
            //
            //     //HandleStab();
            // }
        
    }

    //  private void HandleStab()
    //  {
    //     if (!hasHit)
    //     {
    //         attackDetails[0] = attackDamage;
    //         attackDetails[1] = transform.position.x;
    //         foreach (Collider2D collider in enemiesToDamage)
    //         {
    //             collider.transform.SendMessage("Damage", attackDetails); //Calls enemy script so it will get damage
    //         }
    //
    //         Debug.Log("enemy hit");
    //     }
    // }

    // private void OnDrawGizmos()
    // {
    //     GizmosEx.DrawRect(new Rect(leftUpPosition.transform.position, new Vector2(rightDownPosition.transform.position.x - leftUpPosition.transform.position.x, rightDownPosition.transform.position.y - leftUpPosition.transform.position.y)));
    // }
    
}
