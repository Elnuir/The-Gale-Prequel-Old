using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] float touchDamage;
    private PlayerStats playerStats;
    private float[] attackDetails = new float[2];
    [SerializeField] float touchDamageCooldownBase;
    private float touchDamageCooldown;
    private Animator animator;
    void Start()
    {
        
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        print(other);
        if (other.CompareTag("Player"))
        {
            touchDamageCooldown -= Time.deltaTime;
            if (touchDamageCooldown <= 0)
            {
                attackDetails[0] = touchDamage;
                attackDetails[1] = transform.position.x;
                playerStats = other.GetComponent<PlayerStats>();
                playerStats.SendMessage("Damage", attackDetails);
                touchDamageCooldown = touchDamageCooldownBase;
                animator.SetBool("IsUp", true);
                print("otherEGEGEGEG");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            touchDamageCooldown = 0;
            animator.SetBool("IsUp", false);
        }
    }
}
