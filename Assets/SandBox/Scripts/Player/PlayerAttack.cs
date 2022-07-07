using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //private float timeBtwAttack;
   // public float startTimeBtwAttack;

    public Transform attackPosLeftUp, attackPosRightDown;
    public LayerMask whatIsEnemies;
   // public float attackRange;
   // private const string PLAYER_ATTACK = "CharacterAttack";
   // private const string PLAYER_FLY_ATTACK = "CharacterFlyAttack";

    public float baseAttackDamage;
    public float attackDamage;

    private Player player;
    private PlayerClimb playerClimb;
    private DashMove dashMove;

    //private Animator animator;

    private PlayerStats PS;

    private float[] attackDetails = new float[2];
        //JUMP ATTACK
    public Transform jumpAttackPosLeftUp, jumpAttackPosRightDown;
    private Collider2D[] enemiesToDamage,enemiesToDamageJump;
    Collider2D checkEnemiesToDamageJump;
    private Rigidbody2D rb;
    [SerializeField] float reboundForce;

   // [SerializeField] float attackAnimDurationBase;
   // private float attackAnimTimeLeft;
    
    void Start()
    {
        dashMove = GetComponent<DashMove>();
        attackDamage = baseAttackDamage;
        player = GetComponent<Player>();
        PS = GetComponent<PlayerStats>();
        playerClimb = GetComponent<PlayerClimb>();
        rb = GetComponent<Rigidbody2D>();
       // attackAnimTimeLeft = attackAnimDurationBase;
    }

    // void AttackCheckerDisablerNahui()
    // {
    //     if (player.isAttacking)
    //     {
    //         attackAnimTimeLeft -= Time.deltaTime;
    //         if (attackAnimTimeLeft <= 0)
    //         {
    //             player.isAttacking = false;
    //             attackAnimTimeLeft = attackAnimDurationBase;
    //         }
    //     }
    //     else if (!player.isAttacking)
    //     {
    //         attackAnimTimeLeft = attackAnimDurationBase;
    //     }
    // }

    void Update()
    {
       // AttackCheckerDisablerNahui();
//        print(rb.velocity.y);
       // if (Input.GetMouseButtonDown(0) && player.isGrounded && !player.isDead)
         // if (Input.GetMouseButtonDown(0) && !player.isDead && !playerClimb.isHooked && !dashMove.isDashing)
         // {
         //     if (!GameManager.gameIsPaused)
         //     {
         //         player.isAttacking = true; //Calls player scrips to make animation of attacking to work
         //     }
         // }
        // if(dashMove.isDashing)
        // {
        //     player.isAttacking = false;
        // }

        checkEnemiesToDamageJump = Physics2D.OverlapArea(jumpAttackPosLeftUp.position, jumpAttackPosRightDown.position, whatIsEnemies);
        enemiesToDamageJump = Physics2D.OverlapAreaAll(jumpAttackPosLeftUp.position, jumpAttackPosRightDown.position, whatIsEnemies);
        if (checkEnemiesToDamageJump && !player.isGrounded && !player.isDead && !playerClimb.isHooked && rb.velocity.y < -10f)
        {
            //print("Shit333");
            attackDetails[0] = baseAttackDamage;
            attackDetails[1] = transform.position.x;
            foreach (var collider in enemiesToDamageJump)
            {
                if (collider.GetComponent<JumpKillable>().isJumpKillable)
                {
                    collider.transform.SendMessage("Damage", attackDetails);
                    rb.velocity = Vector2.up * reboundForce;
                }
                

//                print("Shit222");
            }
        }

    }

    // public void Attack()
    // {
    //     enemiesToDamage = Physics2D.OverlapAreaAll(attackPosLeftUp.position, attackPosRightDown.position, whatIsEnemies); //creates circle inside of which enemies get their asses whuped
    //     attackDetails[0] = baseAttackDamage;
    //     attackDetails[1] = transform.position.x;
    //     foreach (Collider2D collider in enemiesToDamage)
    //     {
    //         collider.transform.SendMessage("Damage", attackDetails); //Calls enemy script so it will get damage
    //     }
    // }


    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireCube(new Vector2((attackPosLeftUp.position.x + attackPosRightDown.position.x)/2, (attackPosLeftUp.position.y + attackPosRightDown.position.y)/2),
    //         new Vector2((attackPosRightDown.position.x - attackPosLeftUp.position.x), (attackPosLeftUp.position.y - attackPosRightDown.position.y)));
    //     Gizmos.DrawWireCube(new Vector2((jumpAttackPosLeftUp.position.x + jumpAttackPosRightDown.position.x)/2, (jumpAttackPosLeftUp.position.y + jumpAttackPosRightDown.position.y)/2),
    //         new Vector2((jumpAttackPosRightDown.position.x - jumpAttackPosLeftUp.position.x), (jumpAttackPosLeftUp.position.y - jumpAttackPosRightDown.position.y)));//draws a rectangle inside of which enemies get their asses whuped;
    // }

    private void
        Damage(float[] attackDetails) //PLAYER IS BEING DAMAGED HERE!!!! Yes, in PlayerAttack, what's the problem?
    {
        if (!player.GetDashStatus())
        {
            int direction;
            PS.DecreaseHealth(attackDetails[0]); //YeAH RIGHT FOCKING HERE<-------
            if (attackDetails[1] < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            player.KnockBack(direction);
        }
    }
}