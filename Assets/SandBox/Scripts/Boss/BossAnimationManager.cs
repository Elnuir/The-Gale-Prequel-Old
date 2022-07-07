using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationManager : MonoBehaviour
{
    private Boss boss;
    private EnemyAIPathAndMoveBoss movement;

    private Animator animator;
    private string currentState; 
    private const string BOSS_ATTACK = "BossAttack";
    private const string BOSS_FlY_ATTACK = "RedeerFlyAttack";
    private const string BOSS_FLY = "BossFly";
    private const string BOSS_FLY_MATTER = "BossFlyMatter";
    private const string BOSS_CHARGE = "RedeerCharge";
    private const string BOSS_DEATH = "BossDeath";
    private const string BOSS_HIT = "BossDamaged";
    private const string BOSS_IDLE = "RedeerIdle";
    private const string BOSS_TRANSITION_CHARGE = "BossChargingAttack";
    [SerializeField] private float timeToChargeAttack;
    private ChaserBoss chaser;

    void Start()
    {
        chaser = GetComponentInChildren<ChaserBoss>();
        boss = GetComponent<Boss>();
        movement = GetComponent<EnemyAIPathAndMoveBoss>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        print(currentState);
        if (!boss.isDead && !boss.isChargingAttack && !boss.isAttacking && !boss.isHit)// && boss.isMoving)
        {
            if (!boss.isFaseThree)
            {
                ChangeAnimationState(BOSS_FLY);
            }
            else if (boss.isFaseThree)
            {
                ChangeAnimationState(BOSS_FLY_MATTER);
            }
        }
        else if (boss.isHit && !boss.isDead)
        {
            // print("yayayayay123");
            ChangeAnimationState(BOSS_HIT);
        }
        //else if(boss.isAttacking)
        // ChangeAnimationState(BOSS_ATTACK);
        else if (boss.isChargingAttack && !boss.isDead && !boss.isHit)
        {
            animator.SetBool("Attack", true);
            Invoke(nameof(StopCharging), timeToChargeAttack);
        }
        // ChangeAnimationState(BOSS_ATTACK);
        else if(boss.isDead)
            ChangeAnimationState(BOSS_DEATH);
    }
    
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;              //Used to call anims from animator, kinda useful shit
        animator.Play(newState);
        currentState = newState;
    }

    void StopCharging()
    {
        animator.SetBool("Attack", false);
    }
}
