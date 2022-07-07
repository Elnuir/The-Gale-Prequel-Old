using System;
using UnityEngine;

public class Possessed: MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private GameObject player;

    private Vector2 playerDirection;
    private float distanceToPlayer;
    //public float distanceToStop;

    //public float speed;

    private float currentHealth;
    public float maxHealth;
    public float percantageOfUnding;
    public float percanteOfHealthToGiveForExorcism;

    [SerializeField] private GameObject exorcismCheck, Fpossessed;
    [SerializeField] private Transform touchDamageCheckLeftUp, touchDamageCheckRightDown;
    private int damageDirection; 
    //[SerializeField] private GameObject projectileFireBall;

    private float knockbackStartTime;

    // [SerializeField] private Vector2 knockbackSpeed;
    // [SerializeField] private GameObject hitParticle;
    [SerializeField] private float exorcismRadius, touchDamage, touchDamageCooldown, knockbackDuration;

    private float lastTouchDamageTime;

    // [SerializeField] float startChargingTime;
    // private float chargineTime;
    // [SerializeField] private Transform fireBallSpawnTransform;

    [SerializeField] private LayerMask whatIsPlayer;
    private EnemyAIPathAndMovePossessed pathAndMoverPossessed;
    private ChaserPossessed chaserPossessed;


    public float knockBackForce;

    //public bool isKnockedBack;
    public bool isMoving = true;
    public bool facingRight;
    public bool isDead;
    public bool isAttacking;
    public bool isHit;
    public bool isChargingAttack;
    public bool isBelowExorcism;
    public bool isIdling;

    private float[] attackDetails = new float[2];
    private Collider2D hit;
    private JumpKillable jumpKillable;
    private PossessedTutorialCheck possessedTutorialCheck;
    


    // Start is called before the first frame update
    void Start()
    {
        chaserPossessed = GetComponent<ChaserPossessed>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        pathAndMoverPossessed = GetComponent<EnemyAIPathAndMovePossessed>();
        jumpKillable = GetComponentInChildren<JumpKillable>();
        possessedTutorialCheck = FindObjectOfType<PossessedTutorialCheck>();
        
    }

    // Update is called once per frame
    void Update()
    {
        GetBackCheck();
        if (player == null || !player.activeInHierarchy)
        {
            player = GameObject.Find("Character");
            if (player == null)
            {
                player = GameObject.Find("Character(Clone)");
            }
        }
        
        if (facingRight == false && rigidbody2D.velocity.x > 0  && !isHit && !isDead && pathAndMoverPossessed.target.transform.position.x - gameObject.transform.position.x > 0)
        {
            Flip();
        }
        else if (facingRight && rigidbody2D.velocity.x < 0 && !isHit && !isDead && pathAndMoverPossessed.target.transform.position.x - gameObject.transform.position.x < 0) //Flips
        {
            Flip();
        }

        DieCheck();
    }

    void GetBackCheck()
    {
        if (transform.position.y < -100f)
        {
            transform.position = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        CheckTouchDamage();
       // CheckDistantDamage();
       // DistantAttackAction();
       // CheckDistantDamage();
        // crutch
        SetHitFalse();

    }

    void CheckTouchDamage()
    {
        hit = Physics2D.OverlapArea(touchDamageCheckLeftUp.position, touchDamageCheckRightDown.position, whatIsPlayer);
        if(hit == null)
        {
            isAttacking = false;
//                print("false");
        }
        if (Time.time >= lastTouchDamageTime + touchDamageCooldown)
        {
            if (hit != null)
            {
                if (!isDead && !isHit && pathAndMoverPossessed.groundCheck != null)
                {
                    isAttacking = true;
                    lastTouchDamageTime = Time.time;
//                    print("true");
                }
            }
            
        }
        
        
    }

    public void TouchDamageAnim()
    {
        attackDetails[0] = touchDamage;
        attackDetails[1] = transform.position.x;
        hit.SendMessage("Damage", attackDetails);
    }
    public void DamageReceive(float[] attackDetails)
    {
        if (!isDead && !isBelowExorcism)
        {
            currentHealth -= attackDetails[0];
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
            else if (currentHealth <= (maxHealth/100) * percantageOfUnding)
            {
                isBelowExorcism = true;
                if (possessedTutorialCheck.tutorIsDone == 0)
                {
                    possessedTutorialCheck.tutorBase.SetActive(true);
                }
            }
        }
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

    void DieCheck()
    {
        if (isBelowExorcism)
        {
            Fpossessed.SetActive(true);
            jumpKillable.isJumpKillable = false;
            Collider2D exorcismable =
                Physics2D.OverlapCircle(exorcismCheck.transform.position, exorcismRadius, whatIsPlayer);
            if (exorcismable)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    PlayerStats playerHealth = FindObjectOfType<PlayerStats>();
                    HealthBar healthBar = FindObjectOfType<HealthBar>();
                    playerHealth.currentHealth =
                        Mathf.Clamp(
                            playerHealth.currentHealth -
                            playerHealth.maxHelth / 100 * percanteOfHealthToGiveForExorcism, 1, playerHealth.maxHelth);
                    healthBar.SetHealth(playerHealth.currentHealth);
                    //isBelowExorcism = false;
                    Dead();
                }
            }
        }
    }

    public void Dead()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.EnemyKilledHandler();
        Fpossessed.SetActive(false);
        isDead = true;
        isHit = false;
        isAttacking = false;
        isBelowExorcism = false;
        //rigidbody2D.gravityScale = 3;
        rigidbody2D.velocity = Vector2.zero;
        gameObject.layer = 16;
        Invoke(nameof(AfterDeathPooling), 1.5f);
    }

    private void Flip()
    {
       if(!ActionEx.CheckCooldown(Flip, 0.5f)) return;

       facingRight = !facingRight;
       transform.Rotate(0f, 180f, 0f); // flips
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2((touchDamageCheckLeftUp.position.x + touchDamageCheckRightDown.position.x)/2, (touchDamageCheckLeftUp.position.y + touchDamageCheckRightDown.position.y)/2),
            new Vector2((touchDamageCheckRightDown.position.x - touchDamageCheckLeftUp.position.x), (touchDamageCheckLeftUp.position.y - touchDamageCheckRightDown.position.y)));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(exorcismCheck.transform.position, exorcismRadius);
       // Gizmos.DrawLine(distantDamageCheck.transform.position, transform.position - transform.right*distanceDistantAttack);
        
    }

    void AfterDeathPooling()
    {
        gameObject.layer = 11;
        isDead = false;
        currentHealth = maxHealth;
        facingRight = false;
        jumpKillable.isJumpKillable = true;
        //rigidbody2D.gravityScale = 0;
        gameObject.PutToPool();
    }
    // void CheckDistantDamage()
    // {
    //     if (Time.time >= lastDistantDamageTime + distantDamageCooldown)
    //     {
    //         
    //         RaycastHit2D distanthit = Physics2D.Raycast(distantDamageCheck.transform.position, -transform.right, distanceDistantAttack, whatIsPlayer);
    //
    //         if (distanthit && pathAndMoverNibbler.isGroundedCheckRay && !isDead && !isHit && chaserNibbler.isChasingPlayer && !isAttacking)
    //         {
    //             isChargingAttack = true;
    //             //DistantAttackAction();
    //         }
    //     }
    // }
    // void DistantAttackAction()
    // {
    //     if (isChargingAttack)
    //     {
    //         chargineTime -= Time.deltaTime;
    //         if (chargineTime <= 0)
    //         {
    //             isChargingAttack = false;
    //             chargineTime = startChargingTime;
    //             var p = projectileFireBall.GetCloneFromPool(null);
    //             p.transform.position = fireBallSpawnTransform.position;
    //             p.GetComponent<Fireball>().flyRight = transform.right.x > 0;
    //             lastDistantDamageTime = Time.time;
    //         }
    //     }
    //
    // }
    
}