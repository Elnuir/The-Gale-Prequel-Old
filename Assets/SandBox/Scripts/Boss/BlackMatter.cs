using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlackMatter : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    [SerializeField] private BossHealthBar bossHealthBar;
    private Rigidbody2D rigidbody2D;
    private int damageDirection;
    [SerializeField] private GameObject bossSpheres;
    public bool isHit;
    public bool isDead;
    private bool isStartFucking;
    public float knockBackForce,  knockbackDuration;

    private float lastTouchDamageTime;

    [SerializeField] private float damageRadius, touchDamage, touchDamageCooldown;
    [SerializeField] private LayerMask whatIsPlayer;
    public bool isAttacking;
    private float[] attackDetails = new float[2];

    [SerializeField] private GameObject touchDamageCheck;
    public int spheresAmount = 3;
  //  [SerializeField] Animator deathAnimator;
  [SerializeField] GameObject matterBlackBones;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private GameObject blackMatter;
    public bool isFaseFour;
    [SerializeField] private Transform spotToGoToAfter3Fase;
    private EnemyAIPathAndMoveBlackMatter enemyAIPathAndMoveBlackMatter;
    private ChaserBlackMatter chaserBlackMatter;
    private GameManager gameManager;


    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        enemyAIPathAndMoveBlackMatter = GetComponent<EnemyAIPathAndMoveBlackMatter>();
        chaserBlackMatter = GetComponentInChildren<ChaserBlackMatter>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DamageReceive(float[] attackDetails)
    {
       // if (!isDead && !isInvincibleFirst && !isInvincibleTwo)
        //{
            currentHealth -= attackDetails[0];
            bossHealthBar.SetHealth(currentHealth);
            //Instantiate(hitParticle, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f))); //HITPARTICLE
            if (attackDetails[1] > transform.position.x)
            {
                damageDirection = -1;
            }
            else
            {
                damageDirection = 1;
            }

            //hitParticle
            if (currentHealth > 0.0f)
            {
                KnockBack();
            }
            if (currentHealth <= maxHealth*0.05)
            {
                bossSpheres.SetActive(true);                //FASE FOUR
                isFaseFour = true;
                enemyAIPathAndMoveBlackMatter.target = spotToGoToAfter3Fase;
            }
            
       // }
    }

    private void FixedUpdate()
    {
        SetHitFalse();
        CheckTouchDamage();

    }

    void KnockBack()
    {
        isHit = true;
        hitEffect.Play();
        // isAttacking = false;
        // isMoving = false;
        _setHitFalseTick = Time.time;
        rigidbody2D.AddForce(Vector2.right * damageDirection * knockBackForce);
        //Debug.Log("set true");
    }
    private float _setHitFalseTick = float.MaxValue;
    void SetHitFalse()
    {
        if (isHit && Time.time > _setHitFalseTick + knockbackDuration)
        {
            isHit = false;

            //isAttacking = true;
            //isMoving = true;
            //Debug.Log("set false");
        }
    }
    void CheckTouchDamage()
    {
        if (Time.time >= lastTouchDamageTime + touchDamageCooldown)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, damageRadius, whatIsPlayer);
            
            if (hit != null)
            {
                if (!isHit)
                {
                    isAttacking = true;
                    lastTouchDamageTime = Time.time;
                    attackDetails[0] = touchDamage;
                    attackDetails[1] = transform.position.x;
                    hit.SendMessage("Damage", attackDetails);
                    print("wow1");
                }
            }
            else
            {
                isAttacking = false;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(touchDamageCheck.transform.position, damageRadius);
    //     Gizmos.DrawLine(wallCheck.transform.position, wallCheck.transform.position + transform.right * wallCheckDistance);
    //     Gizmos.DrawLine(groundCheck.transform.position, groundCheck.transform.position  - transform.up * groundCheckDistance);
    //     Gizmos.DrawLine(ceilingCheck.transform.position, ceilingCheck.transform.position + transform.up * ceilingCheckDistance);
    //     // Gizmos.DrawLine(distantDamageCheck.transform.position, transform.position - transform.right*distanceDistantAttackRadius);
    //     Gizmos.DrawWireSphere(distantDamageCheck.transform.position, distanceDistantAttackRadius);
    }
    public void Dead()
    {
        if (spheresAmount <= 0)
        {
            gameManager.EnemyKilledHandler();
            isDead = true;
            isHit = false;
            isAttacking = false;
            //rigidbody2D.gravityScale = 3;
            rigidbody2D.velocity = Vector2.zero;
            gameObject.layer = 16;
            isStartFucking = true;
           // deathAnimator.enabled = true;
            Invoke(nameof(DeathForReal), 6f);
            // Invoke(nameof(AfterDeathPooling), 5f);
        }
    }

    void DeathForReal()
    {
        blackMatter.SetActive(false);
        gameManager.RunWinCanvas();
    }
}
