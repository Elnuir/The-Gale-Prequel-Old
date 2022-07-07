using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformKnifeHook : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            // other.gameObject.transform.SetParent(transform.parent);

            if (other.TryGetComponent<StickTo>(out var follower))
            {
                follower.Target = transform;
                follower.Offset = other.transform.position - transform.position;

                if (other.TryGetComponent<ThrowingKnife>(out var knife))
                {
                    knife.hasHit = true;
                    knife.GetComponent<Rigidbody2D>().isKinematic = true;
                    knife.GetComponent<Rigidbody2D>().velocity = default;
                    knife.gameObject.layer = 15; //Makes it expired
                }
            }


        }
    }
}
