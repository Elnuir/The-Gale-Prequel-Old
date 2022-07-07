using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThrowingKnife : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool hasHit;
    //[SerializeField] private GameObject leftUpPosition;       //For overlapingArea
    //[SerializeField] private GameObject rightDownPosition;    //
    [SerializeField] private float rotationSpeed;
    public LayerMask StopMask;
    //public LayerMask whatIsEnemies;
    private Collider2D[] enemiesToDamage;
    private float[] attackDetails = new float[2];
    private float attackDamage;
    private Throw throw1;
    private KnivesCounter knivesCounter;
    private bool triggered;
    private BuffManager _manager;

    void Start()
    {
        attackDamage = FindObjectOfType<Throw>().attackDamageKnife;
        knivesCounter = FindObjectOfType<KnivesCounter>();
        throw1 = FindObjectOfType<Throw>();
        rb = GetComponent<Rigidbody2D>();
        _manager = FindObjectOfType<BuffManager>();
    }

    // Update is called once per frame
    void Update()
    {

        //enemiesToDamage = Physics2D.OverlapAreaAll(leftUpPosition.transform.position, rightDownPosition.transform.position, whatIsEnemies); //creates square inside of which enemies get their asses whuped

        if (hasHit == false)
        {
            transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // if(other.gameObject.tag == "Enemy")
        //     HandleStab();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.layer == 15 && other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            //   throw1.amountOfKnives++;
            throw1.amountOfKnives++;
            knivesCounter.amount = throw1.amountOfKnives;
            knivesCounter.TextUpdate();
            Destroy(gameObject);
        }

        if (StopMask == (StopMask | (1 << other.gameObject.layer)))
        {
            if (other.TryGetComponent<MovingPlatform>(out _) || other.TryGetComponent<MovingPlatformHook>(out _)
             || other.TryGetComponent<MovingPlatformKnifeHook>(out _)) return;

            hasHit = true;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            gameObject.layer = 15; //Makes it expired
        }

        if (other.gameObject.layer == 12)
        {
            if (!hasHit)
            {
                if (!other.isTrigger || other.gameObject.layer == 22)
                {
                    attackDetails[0] = attackDamage;
                    attackDetails[1] = transform.position.x;
                    other.transform.SendMessage("Damage", attackDetails);
                    AddBurningIfRequired(other.transform);
                    AddFatalLightingIfRequired(other.transform);
                    Debug.Log("Enemyhit");
                }
            }
            //HandleStab();
        }
    }

    private void AddBurningIfRequired(Transform target)
    {
        if(!_manager.CheckActive("fire-fury")) return;
        var damager = target.GetComponents<PeriodicDamage>().FirstOrDefault(c => c.DamagerId == PeriodicDamage.DamageTypes.Fire);

        if (damager != null)
            damager.Impose();
        else
            Debug.Log("Бля сука не могу найти периодический дамагер");

    }

    private void AddFatalLightingIfRequired(Transform target)
    {
        if (!_manager.CheckActive("fatal-lighting")) return;
        var lighting = GetComponent<FatalLighting>();
        lighting.Emit(target);
    }


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

//}
