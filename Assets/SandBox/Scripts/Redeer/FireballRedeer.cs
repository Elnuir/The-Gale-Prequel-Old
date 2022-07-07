using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballRedeer : MonoBehaviour
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
            isProjectileDead = true;
            DeathOfProjectile();
        }
        rb.velocity =transform.right *speed * (flyRight ? -1 : 1);
     //   transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerStats = other.GetComponent<PlayerStats>();
            attackDetails[0] = touchDamage;
            attackDetails[1] = transform.position.x;
            playerStats.SendMessage("Damage", attackDetails);
        }
    }

    void DeathOfProjectile()
    {
        isProjectileDead = false;
        lifeTime = startLifeTime;
        gameObject.PutToPool();
    }
}
