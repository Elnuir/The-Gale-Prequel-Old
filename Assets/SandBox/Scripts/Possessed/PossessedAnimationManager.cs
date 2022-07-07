using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessedAnimationManager : MonoBehaviour
{
    private Possessed possessed;
    private EnemyAIPathAndMovePossessed movement;
    private Animator animator;
    private string currentState; 
    private const string POSSESSD_HIT = "PossessedHit";
    private const string POSSESSED_WALK = "PossessedRun";
   // private const string possessed_FAR_ATTACK = "possessedFarAttack";
    private const string POSSESSED_CLOSE_ATTACK = "PossessedAttack";
    private const string POSSESSED_IDLE = "PossessedIdle";
    private const string POSSESSED_DEATH = "PossessedDeath";
    private const string POSSESSED_BECOME_UNDEAD = "PossessedBecomeUndead";
    
    
    // Start is called before the first frame update
    void Start()
    {
        possessed = GetComponent<Possessed>();
        movement = GetComponent<EnemyAIPathAndMovePossessed>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (possessed.isMoving && !possessed.isDead && !possessed.isAttacking && !possessed.isHit)
            ChangeAnimationState(POSSESSED_WALK);
        else if (possessed.isIdling && !possessed.isMoving && !possessed.isDead && !possessed.isAttacking && !possessed.isHit && !possessed.isChargingAttack)
        {
            ChangeAnimationState(POSSESSED_IDLE);
        }
        else if (possessed.isHit)
            //animator.SetTrigger("knockBack");
            ChangeAnimationState(POSSESSD_HIT);
        // else if(possessed.isChargingAttack)
        //     ChangeAnimationState(possessed_FAR_ATTACK);
        else if(possessed.isAttacking)
            ChangeAnimationState(POSSESSED_CLOSE_ATTACK);
        else if(possessed.isDead)
            ChangeAnimationState(POSSESSED_DEATH);
    }
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;              //Used to call anims from animator, kinda useful shit
        animator.Play(newState);
        currentState = newState;
    }
}
