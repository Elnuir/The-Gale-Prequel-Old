using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private const string PLAYER_IDLE = "CharacterIdle"; //
    private const string PLAYER_JUMP = "CharacterJump"; //
    private const string PLAYER_JUMPFLY = "CharacterJumpFly"; //
    private const string PLAYER_LANDING = "CharacterLanding"; //
    private const string PLAYER_RUN = "CharacterRun"; //
    private const string PLAYER_DEATH = "CharacterDeath"; //
    private const string PLAYER_HIT = "CharacterHit"; //
    private const string PLAYER_ATTACK = "CharacterAttack";
    private const string PLAYER_FLY_ATTACK = "CharacterFlyAttack";
    private const string PLAYER_DASH = "CharacterDash";

    private Animator animator;
    private string currentState;
    private Player player;
    private PlayerCombatManager combatManager;
    //private Rigidbody2D rb;

    private DashMove dashMove;

    private Rigidbody2D rb;
   // private static readonly int Jump = Animator.StringToHash("Jump");
   // private static readonly int YVelocity = Animator.StringToHash("yVelocity");

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        combatManager = GetComponent<PlayerCombatManager>();
        dashMove = GetComponent<DashMove>();
       // rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
//        print(player.rb.velocity.x);
        if (player.isGrounded && !player.isAttacking && !dashMove.isDashing && !player.knockback && !player.isDead) //Fucking with anims
        {
//            print("SHITsssss");
            animator.SetBool("JumpUp", false);
           // bool a = animator.GetBool("JumpDown");
            if (animator.GetBool("JumpDown") && !player.isDead)
            {
                ChangeAnimationState(PLAYER_LANDING);
            }

            else if (animator.GetBool("JumpDown") == false && !player.isDead)
            {
                //animator.SetBool("JumpDown", false);
                if (player.moveInput != 0)
                {
                    ChangeAnimationState(PLAYER_RUN);
                }
                else
                {
                    ChangeAnimationState(PLAYER_IDLE);
                }
            }
        }
        else if (!player.isGrounded && !player.isDead && !player.isAttacking && !dashMove.isDashing && !player.knockback && rb.velocity.y > 1e-05f)
        {
            
                animator.SetBool("JumpUp", true);
                animator.SetBool("JumpDown", false);

                //print(rb.velocity.y);
//            print("DUDUDUDUD");
        }
        else if (!player.isGrounded && !player.isDead && !player.isAttacking && !dashMove.isDashing && !player.knockback)
        {
            if (rb.velocity.y < 1e-05f)
            {
                animator.SetBool("JumpUp", false);
                animator.SetBool("JumpDown", true);
                ChangeAnimationState(PLAYER_JUMPFLY);
            }
        }
       // else if (!player.isGrounded && !dashMove.isDashing && !player.knockback && !player.isDead && !player.isAttacking)
       // {
       //     ChangeAnimationState(PLAYER_JUMPFLY);
        //}
        else if (!player.isGrounded && !dashMove.isDashing && !player.knockback && !player.isDead && player.isAttacking)
        {
            animator.SetBool("JumpUp", false);
            animator.SetBool("JumpDown", false);
            ChangeAnimationState(PLAYER_FLY_ATTACK);
        }
        else if (dashMove.isDashing && !player.isDead)
        {
            ChangeAnimationState(PLAYER_DASH);
        }
        else if (player.isGrounded && player.isAttacking && !player.isDead && !player.isHitted)
        {
            ChangeAnimationState(PLAYER_ATTACK);
        }
        else if (player.knockback)
        {
            if (!player.isDead)
                ChangeAnimationState(PLAYER_HIT);
            else if (player.isDead)
                ChangeAnimationState(PLAYER_DEATH);
        }
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return; //Used to call anims from animator, kinda useful shit
        animator.Play(newState);
        currentState = newState;
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.layer == 7)
    //     {
    //         if (!animator.GetBool("Land"))
    //         {
    //             animator.SetBool("Land", true);
    //         }
    //     }
    //
    //     
    // }
}