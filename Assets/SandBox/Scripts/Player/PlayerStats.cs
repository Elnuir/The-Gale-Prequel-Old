using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private Player player;
    private Animator animator;
   // private const string PLAYER_DEATH = "CharacterDeath";
    //private const string PLAYER_HIT = "CharacterHit";
    [HideInInspector]
    public HealthBar healthBar;
    
    [SerializeField] public float maxHelth;
    [SerializeField] private GameObject deathChunckParticle, deathBloodParticle;
    public float currentHealth;
    private GameManager GM;
    //private ShieldPotionReloading shieldbuff;
    
    
   

    void Start()
    {
        healthBar = FindObjectOfType<HealthBar>();
        //shieldbuff = FindObjectOfType<ShieldPotionReloading>();
      //  currentHealth = maxHelth;
        GM = FindObjectOfType<GameManager>();  //Important to spell correctly
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
       // healthBar.SetMaxHealth(maxHelth);
    }

    public void DecreaseHealth(float amount)
    {
        if (!player.isDead)
        {
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHelth);     //ATTENTION HERE YO
            // if (shieldbuff.isShieldBuffed)
            // {
            //     currentHealth -= amount * (1 - shieldbuff.percentToProtectFrom / 100f);
            // }
            //else
            //{
                currentHealth -= amount;
            //}
            if (amount > 0)
            {
                player.isHitted = true;
                //player.ChangeAnimationState(PLAYER_HIT);
            }

            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0.0f) //DecreasesHealth
            {
                Die();
            }
        }
    }

    private void Die()
    {
        player.isAttacking = false;
        player.knockback = false;
        player.isDead = true;
        gameObject.layer = 13; // 13=dead player, what means it won't collide with enemies after death anymore
        //player.ChangeAnimationState(PLAYER_DEATH);
        Instantiate(deathChunckParticle, transform.position, deathChunckParticle.transform.rotation);
        Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        //GM.Respawn();
        Destroy(gameObject, 5f);
    }

}
