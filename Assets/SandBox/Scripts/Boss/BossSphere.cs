using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSphere : MonoBehaviour
{
    private float currentHealth = 200f;
    private bool isDead;
    [SerializeField] ParticleSystem shards;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Damage(float[] attackDetails)
    {
        if (!isDead)
        {
            currentHealth -= attackDetails[0];

            if (currentHealth <= 0.0f)
            {
                DeathSphere();
            }
        }
    }
    void DeathSphere()
    {
        isDead = true;
        shards.Play();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        FindObjectOfType<EntityHealth>().SpheresAmount--;
        Destroy(gameObject, 3f);
       // Invoke(nameof(AfterDeathPooling), 3f);
    }
}
