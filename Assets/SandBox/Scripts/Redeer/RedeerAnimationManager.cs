using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedeerAnimationManager : MonoBehaviour
{
    private Redeer redeer;
    private EnemyAIPathAndMoveRedeer movement;

    private Animator animator;
    private string currentState; 
    private const string REDEER_ATTACK = "RedeerAttack";
    private const string REDEER_FlY_ATTACK = "RedeerFlyAttack";
    private const string REDEER_RUN = "RedeerRun";
    private const string REDEER_CHARGE = "RedeerCharge";
    private const string REDEER_DEATH = "RedeerDeath";
    private const string REDEER_HIT = "RedeerHit";
    private const string REDEER_IDLE = "RedeerIdle";
    private const string REDEER_TRANSITION_CHARGE = "RedeerTransitionCharge";

    void Start()
    {
        redeer = GetComponent<Redeer>();
        movement = GetComponent<EnemyAIPathAndMoveRedeer>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (redeer.isMoving && !redeer.isDead && !redeer.isAttacking && !redeer.isHit)
            ChangeAnimationState(REDEER_RUN);
        else if (redeer.isHit)
            ChangeAnimationState(REDEER_HIT);
        else if(redeer.isAttacking)
            ChangeAnimationState(REDEER_ATTACK);
        else if(redeer.isChargingAttack)
            ChangeAnimationState(REDEER_FlY_ATTACK);
        else if(redeer.isDead)
            ChangeAnimationState(REDEER_DEATH);
    }
    
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;              //Used to call anims from animator, kinda useful shit
        animator.Play(newState);
        currentState = newState;
    }
}
