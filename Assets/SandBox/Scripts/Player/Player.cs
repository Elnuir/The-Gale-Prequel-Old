using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // private Animator animator;                                    //Animations and their names
    // private string currentState;                                  //
    // private const string PLAYER_IDLE = "CharacterIdle";           //
    // private const string PLAYER_JUMP = "CharacterJump";           //
    // private const string PLAYER_JUMPFLY = "CharacterJumpFly";     //
    // private const string PLAYER_LANDING = "CharacterLanding";    //
    // private const string PLAYER_RUN = "CharacterRun";             //
    // private const string PLAYER_DEATH = "CharacterDeath";          //
    // private const string PLAYER_HIT = "CharacterHit";             //
public enum TypeOfPlayer
{
    Swordsman, Dagger
}
public TypeOfPlayer PlayerType;


    [HideInInspector]
    public Rigidbody2D rb;
    private PlayerClimb playerClimb;


    public float speed; // Movement shit
    [SerializeField] float jumpForce; //
    [HideInInspector]
    public float moveInput; //

    // Ground Checking
    //[SerializeField] Transform groundCheckLeftUp, groundCheckRightDown; //
   // [SerializeField] float checkRadius; // 
    
    [SerializeField] LayerMask whatIsGround; //

    public int extraJumps; //Resposible for double-triple-etc. jumps
    public int extraJumpsValue; // 


    private DashMove dashmove; //ForAnimationOfDashing


    private float knockbackStartTime;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private Vector2 knockbackSpeed;
    private GameManager gameManager; //TO CHECK  if game is paused

    //DELETE TODO:ddd
    private Animator animator;
    public bool facingRight = true; // Responsible for facing right
    public bool isGrounded;
    public bool isSecondJump;

    public bool isRunning;

    // public bool isIdling = true;
    public bool isDead; //for animations, called in PlayerStats
    public bool isHitted; //for animations, called in PlayerStats
    public bool knockback;
    public bool isAttacking; // Crutch, used somewhere in animations
    [HideInInspector] public bool flipRestricted;
    [HideInInspector] public float timeFlipRestricted = 0.3f;
    //private bool secondJumpIsDone;
    private Throw throw1;
    private PlayerWallSlide playerWallSlide;
    private MovementStats movementStats;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerClimb = GetComponent<PlayerClimb>();
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();
        // animator = GetComponent<Animator>();
        dashmove = GetComponent<DashMove>();
        gameManager = FindObjectOfType<GameManager>();
        throw1 = GetComponentInChildren<Throw>();
        playerWallSlide = GetComponent<PlayerWallSlide>();
        movementStats = GetComponent<MovementStats>();
    }

    public void PlayerStates()
    {
        if (moveInput != 0 && isGrounded && !playerWallSlide.wallJumping && !playerWallSlide.isHooked) //Mathf.Abs(rb.velocity.x) > 1e-5f && isGrounded && moveInput != 0)
        {
            isRunning = true;
            //  isIdling = false;
        }
        else if (moveInput == 0 && isGrounded && !playerWallSlide.wallJumping && !playerWallSlide.isHooked) //Mathf.Abs(rb.velocity.x) < 1e-5f && moveInput == 0)
        {
            isRunning = false;
            // isIdling = true;
        }

        if (extraJumps == 0) // && !isGrounded)
        {
            print("EXTRAJUMP0");
          //  if (!secondJumpIsDone)
               // isSecondJump = true;
               isSecondJump = true;
        }
       // else if (extraJumps == extraJumpsValue) //|| isGrounded)
       // {
           // isSecondJump = false;
          
       // }

        animator.SetBool("hit", knockback);
        animator.SetBool("die", isDead);
        animator.SetBool("move", isRunning);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("attack", isAttacking);
       // animator.SetBool("isHooked", playerClimb.isHooked);
        animator.SetBool("isHooked", playerWallSlide.isHooked);
        animator.SetBool("isDashing", dashmove.isDashing);
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isSecondJump", isSecondJump);
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        PlayerStates();
        CheckKnockBack();
        FlipRestrictedCheck();
       // JumpChecker();
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0 && !isDead)
        {
            JumpPerform(); ;
        }
        else 
            JumpChecker();
        
    }
    void JumpChecker()
    {
        if (!GameManager.gameIsPaused)
        {
            if (isGrounded && extraJumps != extraJumpsValue && rb.velocity.y < 0.1f) //|| playerClimb.isHooked)
            {
                extraJumps = extraJumpsValue; //Sets amount of extra jumps when you are on the ground
                isSecondJump = false;
            }
            
            
        }
    }
    public void JumpPerform()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);//Vector2.up * jumpForce; //makes extra jump
        extraJumps--;
    }
    private void FixedUpdate()
    {
        isGrounded = movementStats.IsGrounded;
      //  JumpChecker();
        //isGrounded = Physics2D.OverlapArea(groundCheckLeftUp.position, groundCheckRightDown.position, whatIsGround); //creates circle which suppose to be at feet
        if (!knockback && !isDead && !playerClimb.isHooked && !playerWallSlide.isHooked && !playerWallSlide.wallJumpPerformed)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y); //makes character move
            //   print("VELOCITY");
        }

        else if (playerClimb.isHooked || playerWallSlide.isHooked)
        {
            rb.velocity = Vector2.zero;
            //    print("VELOCITY");
            rb.gravityScale = 0;
        }

        if (facingRight == false && moveInput > 0 && !playerClimb.isHooked && !playerWallSlide.isHooked)
        {
            if (!isDead)
                Flip();
        }
        else if (facingRight && moveInput < 0 && !playerClimb.isHooked && !playerWallSlide.isHooked) //Flips character
        {
            if (!isDead)
                Flip();
        }
    }

    void FlipRestrictedCheck()
    {
        if (flipRestricted)
        {
            timeFlipRestricted -= Time.deltaTime;
            if (timeFlipRestricted <= 0f)
            {
                flipRestricted = false;
                timeFlipRestricted = 0.3f;
            }
            
        }
    }

    

    

    

    

    public void Flip()
    {
        if (!flipRestricted && !playerWallSlide.wallJumpPerformed)
        {
            facingRight = !facingRight;
            transform.Rotate(0f, 180f, 0f); // flips character
        }
    }

    private void OnDrawGizmos()
    {
       // Gizmos.DrawWireCube(new Vector2((groundCheckLeftUp.position.x + groundCheckRightDown.position.x)/2, (groundCheckLeftUp.position.y + groundCheckRightDown.position.y)/2),
       //     new Vector2((groundCheckRightDown.position.x - groundCheckLeftUp.position.x), (groundCheckLeftUp.position.y - groundCheckRightDown.position.y)));
    }

    public void ChangeAnimationState(string newState)
    {
        //  if (currentState == newState) return;              //Used to call anims from animator, kinda useful shit
        // animator.Play(newState);
        // currentState = newState;
    }

    public void StopAttack()
    {
        isAttacking = false; //Crutch, used as an event in animation time line
    }

    public void StopBeingHit()
    {
        isHitted = false; //Crutch, used as an event in animation time line
    }

    public void StopLanding()
    {
//        animator.SetBool("JumpDown", false);
    }

    public void StopJumpUp()
    {
        //  animator.SetBool("JumpUp", false);
    }

    // public void StopSecondJump()
    // {
    //     isSecondJump = false;
    //     animator.SetBool("isSecondJump", isSecondJump);
    //     secondJumpIsDone = true;
    // }

    public void KnockBack(int direction)
    {
        isAttacking = false;
        knockback = true;
        animator.SetBool("hit", knockback);
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    void CheckKnockBack()
    {
        if (Time.time > knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            animator.SetBool("hit", knockback);
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
           // print("VELOCITY");
        }
    }

    public bool GetDashStatus()
    {
        return dashmove.isDashing;
    }

    public void ShootKnife()
    {
        throw1.ShootKnife();
    }
    public void ShootSaintWater()
    {
        throw1.ShootSaintWater();
    }
}