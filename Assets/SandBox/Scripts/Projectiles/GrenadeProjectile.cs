using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GrenadeProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasHit;
    [SerializeField] private float rotationSpeed, explosionRadius;
    public LayerMask StopMask;
    public LayerMask whatToExplode;
    private Collider2D[] enemiesToExplode;

    [SerializeField] private Transform explosionCenter;
    
    private Throw throw1;
    
    private bool triggered;
    [SerializeField] ParticleSystem squirt, shards;
    private bool squirtPlayed;

    void Start()
    {
        throw1 = FindObjectOfType<Throw>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!hasHit && StopMask == (StopMask | (1 << other.gameObject.layer)))
        {
            hasHit = true;
            
            if (!squirtPlayed)
            {
                shards.Play();
                squirt.Play();
                squirtPlayed = true;
            }

            //SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            //sprite.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Invoke(nameof(CommitExplosion), 1f);
            Destroy(gameObject, 2f);
        }
    }

    void CommitExplosion()
    {
        if (hasHit)
        {
            enemiesToExplode =
                Physics2D.OverlapCircleAll(explosionCenter.transform.position, explosionRadius, whatToExplode);
            foreach (var enemy in enemiesToExplode)
            {
                if (enemy != null && enemy.gameObject != null)
                {
                    print("ENemyBobmed");
                   // enemy.GetComponent<NPossessedHealth>().Dead(true);
                   enemy.BroadcastMessage("Damage", new float[] { 999, 0 });
                   // TODO: HARDCODED VALUE
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(explosionCenter.transform.position, explosionRadius);
    }
}
