using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NibblerAnimationManager : MonoBehaviour
{
    private Nibbler nibbler;
    private EnemyAIPathAndMoveNibbler movement;
    private Animator animator;
    private string currentState; 
    private const string NIBBLER_HIT = "NibblerHit";
    private const string NIBBLER_WALK = "NibblerWalk";
    private const string NIBBLER_FAR_ATTACK = "NibblerFarAttack";
    private const string NIBBLER_CLOSE_ATTACK = "NibblerCloseAttack";
    private const string NIBBLER_IDLE = "NibblerIdle";
    private const string NIBBLER_DEATH = "NibblerDeath";


    // Start is called before the first frame update
    void Start()
    {
        nibbler = GetComponent<Nibbler>();
        movement = GetComponent<EnemyAIPathAndMoveNibbler>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (nibbler.isMoving && !nibbler.isDead && !nibbler.isAttacking && !nibbler.isHit)
            ChangeAnimationState(NIBBLER_WALK);
        else if (nibbler.isIdling && !nibbler.isMoving && !nibbler.isDead && !nibbler.isAttacking && !nibbler.isHit && !nibbler.isChargingAttack)
        {
            ChangeAnimationState(NIBBLER_IDLE);
        }
        else if (nibbler.isHit)
            //animator.SetTrigger("knockBack");
            ChangeAnimationState(NIBBLER_HIT);
        else if(nibbler.isChargingAttack)
            ChangeAnimationState(NIBBLER_FAR_ATTACK);
        else if (nibbler.isAttacking)
        {
            ChangeAnimationState(NIBBLER_CLOSE_ATTACK);
        }
        else if(nibbler.isDead)
            ChangeAnimationState(NIBBLER_DEATH);
    }
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;              //Used to call anims from animator, kinda useful shit
        animator.Play(newState);
        currentState = newState;
    }
}
