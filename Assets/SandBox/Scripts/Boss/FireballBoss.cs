using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBoss : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float speed;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float touchDamage;
    private float[] attackDetails = new float[2];
    [SerializeField] private float startLifeTime;
    private float lifeTime;
    private bool isProjectileDead;
    [HideInInspector] public bool flyRight = true;
    [SerializeField] private ParticleSystem shards;
    private float currentHealth = 1;
    
    private PlayerStats playerStats;

    //private Vector2 InitDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        lifeTime = startLifeTime;
       // InitDirection = new Vector2(Math.Sign(transform.right.x), 1);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0 && !isProjectileDead)
        {
            DeathOfProjectile();
        }
        rb.velocity =transform.right *speed * (flyRight ? -1 : 1);
     //   transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.GetComponent<DashMove>().isDashing)
        {
            playerStats = other.GetComponent<PlayerStats>();
            attackDetails[0] = touchDamage;
            attackDetails[1] = transform.position.x;
            playerStats.SendMessage("Damage", attackDetails);
            DeathOfProjectile();
//            print("WOW");
        }
    }

    void DeathOfProjectile()
    {
        isProjectileDead = true;
        shards.Play();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<TrailRenderer>().enabled = false;
        Invoke(nameof(AfterDeathPooling), 3f);


    }

    void AfterDeathPooling()
    {
        isProjectileDead = false;
        lifeTime = startLifeTime;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<TrailRenderer>().enabled = true;
        gameObject.PutToPool();
    }
    public void Damage(float[] attackDetails)
    {
        if (!isProjectileDead)
        {
            currentHealth -= attackDetails[0];
            if (currentHealth <= 0.0f)
            {
                DeathOfProjectile();
            }
        }
    }
}
