using System.Linq.Expressions;
using Pathfinding;
using UnityEngine;


public class Boss : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private GameObject player;

    private Vector2 playerDirection;
    private float distanceToPlayer;
    //public float distanceToStop;

    //public float speed;

    private float currentHealth;
    public float maxHealth;

    //[SerializeField] private GameObject groundCheck, ceilingCheck; // wallCheck,;// touchDamageCheck,;
    private int damageDirection;

    private float knockbackStartTime;

    // [SerializeField] private Vector2 knockbackSpeed;
    // [SerializeField] private GameObject hitParticle;
    [SerializeField] private float damageRadius, touchDamage, touchDamageCooldown, knockbackDuration, wallCheckDistance, groundCheckDistance, ceilingCheckDistance;

    private float lastTouchDamageTime;

    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;
    private EnemyAIPathAndMoveBoss targetToFlip;


    public float knockBackForce;

    //public bool isKnockedBack;
    public bool isMoving = true;
    public bool facingRight;
    public bool isDead;
    public bool isAttacking;
    public bool isHit;
    public bool isInvincibleFirst, isInvincibleTwo;
    public bool isFaseTwo;

   // float[] damageAfterSecondFaseCommitment; //MUST LEAVE LESS THAN 1/3 OF HEALTH
    [SerializeField] private float damageAfterSecondFase;
    public bool isFaseThree;

    private float[] attackDetails = new float[2];
    
    
    [SerializeField] private GameObject projectileFireBall;
    private float chargineTime;
    public bool isChargingAttack;
    private float lastDistantDamageTime;
    [SerializeField] private float distantDamageCooldown;
    [SerializeField] private float distanceDistantAttackRadius;         //FAR ATTACK
    [SerializeField] private GameObject  distantDamageCheck;
    private ChaserBoss chaserBoss;
    [SerializeField] private Transform fireBallSpawnTransform;
    [SerializeField] float startChargingTime;
    [SerializeField] private GameObject spawners;
    [SerializeField] private WaveCounter waveCounter;
        // [SerializeField] private GameObject bossSpheres;
    [SerializeField] private GameObject oldMap, newMap;
    
    [SerializeField] private BossHealthBar bossHealthBar;
    [SerializeField] private GameObject blackMatter;
    [SerializeField] private GameObject notBlackMatter;
    [SerializeField] private GameObject shield;
    

    bool isPaused; //FOR SPAWNERS

    [SerializeField]private Transform positionToGoToAfterFirstFase;


    // Start is called before the first frame update
    void Start()
    {
        bossHealthBar = FindObjectOfType<BossHealthBar>();
        bossHealthBar.SetHealth(maxHealth);
        chaserBoss = GetComponentInChildren<ChaserBoss>();
        chargineTime = startChargingTime;
        rigidbody2D = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        targetToFlip = GetComponent<EnemyAIPathAndMoveBoss>();
    }

    // Update is called once per frame
    void Update()
    {
        //      print(maxHealth * 3 / 4);
//        print(currentHealth <= maxHealth * 3 / 4);
        
        CheckDistantDamage();
        DistantAttackAction();
        if (player == null || !player.activeInHierarchy)
        {
            player = FindObjectOfType<Player>().gameObject;
            Debug.Log("Player is " + player.gameObject) ;
            if (player == null)
            {
                player = GameObject.Find("Character(Clone)");
            }
        }

        if (facingRight == false && rigidbody2D.velocity.x > 0 && !isHit && !isDead && targetToFlip.target.transform.position.x - gameObject.transform.position.x > 0)
        {
            Flip();
        }
        else if (facingRight && rigidbody2D.velocity.x < 0 && !isHit && !isDead && targetToFlip.target.transform.position.x - gameObject.transform.position.x < 0) //Flips
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        InvincibleChecker();
       // CheckTouchDamage();
        // crutch
        SetHitFalse();

    }

    // void CheckTouchDamage()
    // {
    //     if (Time.time >= lastTouchDamageTime + touchDamageCooldown)
    //     {
    //         Collider2D hit = Physics2D.OverlapCircle(transform.position, damageRadius, whatIsPlayer);
    //
    //         if (hit != null)
    //         {
    //             if (!isDead && !isHit)
    //             {
    //                 isAttacking = true;
    //                 lastTouchDamageTime = Time.time;
    //                 attackDetails[0] = touchDamage;
    //                 attackDetails[1] = transform.position.x;
    //                 hit.SendMessage("Damage", attackDetails);
    //             }
    //         }
    //         else
    //         {
    //             isAttacking = false;
    //         }
    //     }
    // }
    void CheckDistantDamage()
    {
        if (Time.time >= lastDistantDamageTime + distantDamageCooldown)
        {
            
            Collider2D distanthit = Physics2D.OverlapCircle(distantDamageCheck.transform.position, distanceDistantAttackRadius, whatIsPlayer);

            if (distanthit  && !isDead && !isHit && !isAttacking && !isFaseThree && (!chaserBoss.isOnSpot || isFaseTwo)) //NOT ATTACKING WHEN ON SPOT
            {
//                print(distanthit);
                isChargingAttack = true;
                //DistantAttackAction();
            }
        }
    }

    void DistantAttackAction()
    {
        if (isChargingAttack)
        {
            chargineTime -= Time.deltaTime;
//            print(chargineTime);
            if (chargineTime <= 0)
            {
            
                isChargingAttack = false;
                chargineTime = startChargingTime;
               // var p = projectileFireBall.GetCloneFromPool(null);
               // p.transform.position = fireBallSpawnTransform.position;

                //p.transform.rotation = fireBallSpawnTransform.rotation;
              //  p.GetComponent<FireballBoss>().flyRight = transform.right.x > 0;
                lastDistantDamageTime = Time.time;
            }
        }

    }

    public void MakeFireballAnim()
    {
        var p = projectileFireBall.GetCloneFromPool(null);
        p.transform.rotation = transform.rotation;
        p.transform.position = fireBallSpawnTransform.position;
    }


    public void DamageReceive(float[] attackDetails)
    {
        if (!isDead && !isInvincibleFirst && !isInvincibleTwo)
        {
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
            if (currentHealth <= maxHealth*3/4 && !isFaseThree)
            {
                spawners.SetActive(true);                   //FASE TWO CALL
                isInvincibleFirst = true;
                isFaseTwo = true;
                shield.SetActive(true);
                chaserBoss.isOnSpot = false;
                targetToFlip.target = positionToGoToAfterFirstFase;
            }
            // if (currentHealth <= maxHealth*0.05)
            // {
            //     isInvincibleTwo = true; //FASE FOUR
            // }
            
        }
    }

    void InvincibleChecker()
    {
        if (isInvincibleFirst)
        {
            if (waveCounter.GetComponent<WaveCounter>().currentWave >= 2)        //FASE TWO
            {
                spawners.SetActive(false);
                currentHealth -= damageAfterSecondFase;
                
                Debug.Log("Minus 450");
                
                bossHealthBar.SetHealth(currentHealth);
                FaseThree();                                                        //FASE THREE CALL
                isInvincibleFirst = false; //TODO: EFFECT AND HeALTH BAR
            }
        }
    }

    void FaseThree()
    {
        // targetToFlip.enabled = false;
        // GetComponent<AIPath>().enabled = false;
        // chaserBoss.enabled = false;
        // GetComponent<CapsuleCollider2D>().isTrigger = true;
        // GetComponent<SpriteRenderer>().enabled = false;
        notBlackMatter.SetActive(false);
        blackMatter.SetActive(true);
        blackMatter.GetComponentInChildren<EntityHealth>().HealthBar.SetMaxHealth(maxHealth);
        blackMatter.GetComponentInChildren<EntityHealth>().Health = currentHealth;
        //GetComponent<BossFaseThreeMover>().enabled = true;
        //GetComponent<Observer>().enabled = true;
        GetComponent<Animator>().SetLayerWeight(0, 0f);                 //FASE THREE
        GetComponent<Animator>().SetLayerWeight(1, 1f);
        newMap.SetActive(true);
        oldMap.SetActive(false);
        isFaseThree = true;
        FindObjectOfType<Player>().gameObject.transform.position = new Vector3(6.97f, -12, 0f);
        gameObject.SetActive(false);
    }

    void KnockBack()
    {
        isHit = true;
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
    

    

    private void Flip()
    {
       // if (GetComponent<Observer>().enabled == false)
        //{
            if (!ActionEx.CheckCooldown(Flip, 0.5f)) return;

            facingRight = !facingRight;
            transform.Rotate(0f, 180f, 0f); // flips
       // }

    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(touchDamageCheck.transform.position, damageRadius);
       // Gizmos.DrawLine(wallCheck.transform.position, wallCheck.transform.position + transform.right * wallCheckDistance);
        //Gizmos.DrawLine(groundCheck.transform.position, groundCheck.transform.position  - transform.up * groundCheckDistance);
       // Gizmos.DrawLine(ceilingCheck.transform.position, ceilingCheck.transform.position + transform.up * ceilingCheckDistance);
       // Gizmos.DrawLine(distantDamageCheck.transform.position, transform.position - transform.right*distanceDistantAttackRadius);
       Gizmos.DrawWireSphere(distantDamageCheck.transform.position, distanceDistantAttackRadius);
    }

    void AfterDeathPooling()
    {
        gameObject.layer = 12;
        isDead = false;
        currentHealth = maxHealth;
        facingRight = false;
        rigidbody2D.gravityScale = 0;
        gameObject.PutToPool();
    }
    
}