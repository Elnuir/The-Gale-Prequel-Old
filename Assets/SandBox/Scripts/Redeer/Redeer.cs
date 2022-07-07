using UnityEngine;

public class Redeer : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private GameObject player;

    private Vector2 playerDirection;

    private float distanceToPlayer;
    //public float distanceToStop;

    //public float speed;

    private float currentHealth;
    public float maxHealth;

    [SerializeField] GameObject touchDamageCheck, wallCheck, groundCheck, ceilingCheck;
    private int damageDirection;

    private float knockbackStartTime;

    // [SerializeField] private Vector2 knockbackSpeed;
    // [SerializeField] private GameObject hitParticle;
    [SerializeField] private float damageRadius,
        touchDamage,
        touchDamageCooldown,
        knockbackDuration,
        wallCheckDistance,
        groundCheckDistance,
        ceilingCheckDistance;

    private float lastTouchDamageTime;

    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;
    private EnemyAIPathAndMoveRedeer targetToFlip;


    public float knockBackForce;

    //public bool isKnockedBack;
    public bool isMoving = true;
    public bool facingRight;
    public bool isDead;
    public bool isAttacking;
    public bool isHit;

    private float[] attackDetails = new float[2];


    [SerializeField] private GameObject projectileFireBall;
    private float chargineTime;
    public bool isChargingAttack;
    private float lastDistantDamageTime;
    [SerializeField] private float distantDamageCooldown;
    [SerializeField] private float distanceDistantAttackRadius; //FAR ATTACK
    [SerializeField] private GameObject distantDamageCheck;
    private ChaserRedeer chaserRedeer;
    [SerializeField] private Transform fireBallSpawnTransform;
    [SerializeField] float startChargingTime;
    private JumpKillable jumpKillable;
    public bool isJustSpawned = true;

    [SerializeField] float justSpawnedTimeBase;
    float justSpawnedTimeLeft;


    // Start is called before the first frame update
    void Start()
    {
        chaserRedeer = GetComponentInChildren<ChaserRedeer>();
        chargineTime = startChargingTime;
        rigidbody2D = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        targetToFlip = GetComponent<EnemyAIPathAndMoveRedeer>();
        jumpKillable = GetComponentInChildren<JumpKillable>();
        justSpawnedTimeLeft = justSpawnedTimeBase;
    }

    // Update is called once per frame
    void Update()
    {
        JustSpawnedCounter();
        CheckDistantDamage();
        DistantAttackAction();
        if (player == null || !player.activeInHierarchy)
        {
            player = GameObject.Find("Character");
            if (player == null)
            {
                player = GameObject.Find("Character(Clone)");
            }
        }

        if (facingRight == false && rigidbody2D.velocity.x > 0 && !isHit && !isDead &&
            targetToFlip.target.transform.position.x - gameObject.transform.position.x > 0)
        {
            Flip();
        }
        else if (facingRight && rigidbody2D.velocity.x < 0 && !isHit && !isDead &&
                 targetToFlip.target.transform.position.x - gameObject.transform.position.x < 0) //Flips
        {
            Flip();
        }
    }

    void JustSpawnedCounter()
    {
        if (isJustSpawned)
        {
            justSpawnedTimeLeft -= Time.deltaTime;
            if (justSpawnedTimeLeft <= 0)
            {
                isJustSpawned = false;
                justSpawnedTimeLeft = justSpawnedTimeBase;
            }
        }
    }

    private void FixedUpdate()
    {
        CheckTouchDamage();
        // crutch
        SetHitFalse();
    }

    void CheckTouchDamage()
    {
        if (Time.time >= lastTouchDamageTime + touchDamageCooldown)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, damageRadius, whatIsPlayer);

            if (hit != null)
            {
                if (!isDead && !isHit)
                {
                    isAttacking = true;
                    lastTouchDamageTime = Time.time;
                    attackDetails[0] = touchDamage;
                    attackDetails[1] = transform.position.x;
                    hit.SendMessage("Damage", attackDetails);
                }
            }
            else
            {
                isAttacking = false;
            }
        }
    }

    void CheckDistantDamage()
    {
        if (Time.time >= lastDistantDamageTime + distantDamageCooldown)
        {
            Collider2D distanthit = Physics2D.OverlapCircle(distantDamageCheck.transform.position,
                distanceDistantAttackRadius, whatIsPlayer);

            if (distanthit && !isDead && !isHit && chaserRedeer.isChasingPlayer && !isAttacking)
            {
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
            if (chargineTime <= 0)
            {
                isChargingAttack = false;
                chargineTime = startChargingTime;
                var p = projectileFireBall.GetCloneFromPool(null);
                p.transform.position = fireBallSpawnTransform.position;
                //p.transform.rotation = fireBallSpawnTransform.rotation;
                p.GetComponent<FireballRedeer>().flyRight = transform.right.x > 0;
                lastDistantDamageTime = Time.time;
            }
        }
    }


    public void DamageReceive(float[] attackDetails)
    {
        if (!isDead)
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
            else if (currentHealth <= 0.0f)
            {
                Dead();
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

    void Dead()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.EnemyKilledHandler();
        isDead = true;
        isHit = false;
        isAttacking = false;
        jumpKillable.isJumpKillable = false;
        // rigidbody2D.gravityScale = 3;
        rigidbody2D.velocity = Vector2.zero;
        gameObject.layer = 16;
        Invoke(nameof(AfterDeathPooling), 1.5f);
    }

    public void Flip()
    {
        if (!ActionEx.CheckCooldown(Flip, 0.5f)) return;

        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f); // flips
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(touchDamageCheck.transform.position, damageRadius);
        Gizmos.DrawLine(wallCheck.transform.position,
            wallCheck.transform.position + transform.right * wallCheckDistance);
        Gizmos.DrawLine(groundCheck.transform.position,
            groundCheck.transform.position - transform.up * groundCheckDistance);
        Gizmos.DrawLine(ceilingCheck.transform.position,
            ceilingCheck.transform.position + transform.up * ceilingCheckDistance);
        // Gizmos.DrawLine(distantDamageCheck.transform.position, transform.position - transform.right*distanceDistantAttackRadius);
        Gizmos.DrawWireSphere(distantDamageCheck.transform.position, distanceDistantAttackRadius);
    }

    void AfterDeathPooling()
    {
        gameObject.layer = 11;
        isDead = false;
        currentHealth = maxHealth;
        facingRight = false;
        jumpKillable.isJumpKillable = true;
        rigidbody2D.gravityScale = 0;
        gameObject.PutToPool();
    }
}