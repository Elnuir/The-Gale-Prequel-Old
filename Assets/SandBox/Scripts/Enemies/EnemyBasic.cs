using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

public class EnemyBasic : MonoBehaviour
{
    // public int health;
    // public float speed;
    // private Rigidbody2D rb;
    // private float dazedTime;
    // public float startDazedTime;
    // void Start()
    // {
    //     rb = GetComponent<Rigidbody2D>();
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     if (dazedTime <= 0)
    //     {
    //         speed = 5;
    //     }
    //     else
    //     {
    //         speed = 0;
    //         dazedTime -= Time.deltaTime;
    //     }
    //     rb.velocity = new Vector2(Random.Range(-3, 3), 0)*speed;
    //     if (health <= 0)
    //     {
    //         Destroy(gameObject);
    //     }
    // }
    //
    // public void TakeDamage(int damage)
    // {
    //     dazedTime = startDazedTime;
    //     health -= damage;
    //     print("DamageTaken");
    // }
    //
    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.tag == "Player")
    //     {
    //         Player player = other.gameObject.GetComponent<Player>();
    //         player.ChangeHealth(-1);
    //         print("Hit");
    //     }
    // }
    private enum State
    {
        Moving,
        Knockback,
        Dead
    }
    private State currentState;

    [SerializeField] private float
        groundCheckDistance,
        wallCheckDistance,
        movementSpeed,
        maxHealth,
        knockbackDuration,
        touchDamageCooldown,
        touchDamage,
        touchDamageWidth,
        touchDamageHeight;

    [SerializeField]
    private Transform
        //groundCheck,
        wallCheck,
        touchDamageCheck;

    [SerializeField] private LayerMask
        whatIsGround,
        whatIsPlayer;
    [SerializeField] private Vector2 knockbackSpeed;

    [SerializeField] private GameObject
        hitParticle,
        deathChunkParticle,
        deathBloodParticle;

    private float 
        currentHealth,
        knockbackStartTime,
        lastTouchDamageTime;
    private  float[] attackDetails = new float[2];

    private int 
        facingDirection,
        damageDirection;

    private Vector2
        movement,
        touchDamageBotLeft,
        touchDamageTopRight;
      //  playerDirection;


    private bool 
       // groundDetected, 
        wallDetected;

    private GameObject alive;
    private Rigidbody2D aliveRb;
    private Animator aliveAnim;
    
    private GameObject player;
    private float playerDirection;
    private bool facingRight;

    private void Start()
    {
        
        alive =gameObject;
        aliveRb = alive.GetComponent<Rigidbody2D>();
        aliveAnim = alive.GetComponent<Animator>();

        currentHealth = maxHealth;
        facingDirection = 1;
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Character");
            print("Blyat");
        }

        playerDirection = (player.transform.position - gameObject.transform.position).normalized.x;

        //  player = GameObject.Find("Character");
      //  playerDirection = (transform.position - player.transform.position).normalized;
        switch (currentState)
        {
            case State.Moving:
                UpdateMovingState();
                break;
            case  State.Knockback:
                UpdateKnockbackState();
                break;
            case State.Dead:
                UpdateDeadState();
                break;
        }
    }

    
    //MOVING STATE
    private void EnterMovingState()
    {
        
    }

    private void UpdateMovingState()
    {
       // groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        
        CheckTouchDamage();
       // if (!groundDetected || wallDetected)
        //{
       //     Flip();
       // }
       // else
      
       // {
            movement.Set(movementSpeed*playerDirection*facingDirection, aliveRb.velocity.y);
            aliveRb.velocity = movement;
            if (aliveRb.velocity.x > 0 || facingRight)
            {
                Flip();
            }
            if (aliveRb.velocity.x < 0 || !facingRight)
            {
                Flip();
            }
       // }
    }
    private void ExitMovingState()
    {
        
    }
    //KNOCKBACK STATE
    private void EnterKnockbackState()
    {
        knockbackStartTime = Time.time;
        movement.Set(knockbackSpeed.x * damageDirection, knockbackSpeed.y);
        aliveRb.velocity = movement;
        aliveAnim.SetBool("knockBack", true);
    }

    private void UpdateKnockbackState()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration)
        {
            SwitchState(State.Moving);
        }
    }
    private void ExitKnockbackState()
    {
        aliveAnim.SetBool("knockBack", false);
    }
    //DEAD STATE
    private void EnterDeadState()
    {
        Instantiate(deathChunkParticle, alive.transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, alive.transform.position, deathBloodParticle.transform.rotation);
        Destroy(gameObject);
    }

    private void UpdateDeadState()
    {
        
    }
    private void ExitDeadState()
    {
        
    }
    //OTHER FUNCTIONS
    public void Damage(float[] attackDetails)
    {
        currentHealth -= attackDetails[0];
        Instantiate(hitParticle, alive.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        if (attackDetails[1] > alive.transform.position.x)
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
            SwitchState(State.Knockback);
        }
        else if (currentHealth <= 0.0f)
        {
            SwitchState(State.Dead);
        }
    }

    private void CheckTouchDamage()
    {
        if (Time.time >= lastTouchDamageTime + touchDamageCooldown)
        {
            touchDamageBotLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
            touchDamageTopRight.Set(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

            Collider2D hit = Physics2D.OverlapArea(touchDamageBotLeft, touchDamageTopRight, whatIsPlayer);
            

            if (hit != null)
            {
                lastTouchDamageTime = Time.time;
                attackDetails[0] = touchDamage;
                attackDetails[1] = alive.transform.position.x;
                hit.SendMessage("Damage", attackDetails);
            }
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f,180f,0f);  // flips character
    }
    void SwitchState(State state)
    {
        switch (currentState)
        {
            case State.Moving:
                ExitMovingState();
                break;
            case State.Knockback:
                ExitKnockbackState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
                
        }
        switch (state)
        {
            case State.Moving:
                EnterMovingState();
                break;
            case State.Knockback:
                EnterKnockbackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
                
        }

        currentState = state;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Vector2 botLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 botRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 topRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));;
        Vector2 topLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));;
        
        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
    }
}
