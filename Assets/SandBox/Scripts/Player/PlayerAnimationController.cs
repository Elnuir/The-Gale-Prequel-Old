using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    #region Attributes

    private Animator animator;
    private Player player;
    private const string IDLE_ANIMATION_BOOL = "idle";
    private const string DEATH_ANIMATION_BOOL = "die";
    private const string ATTACK_ANIMATION_BOOL = "attack";
    private const string MOVE_ANIMATION_BOOL = "move";
    private const string HIT_ANIMATION_BOOL = "hit";

    #endregion

    #region Animate Functions

    private void Update()
    {
        // if (player.isDead)
        // {
        //     Animate(DEATH_ANIMATION_BOOL);
        // }
        // if (player.isHitted)
        // {
        //     Animate(HIT_ANIMATION_BOOL);
        // }
        // if (player.isIdling)
        // {
        //     Animate(IDLE_ANIMATION_BOOL);
        // }
        // if (player.isRunning)
        // {
        //     Animate(MOVE_ANIMATION_BOOL);
        // }
        //
        // if (player.isAttacking)
        // {
        //     Animate(ATTACK_ANIMATION_BOOL);
        // }

        

        

        
    }

    #endregion
    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    private void Animate(string boolName)
    {
        DisableOtherAnimations(animator, boolName);
        animator.SetBool(boolName, true);
    }

    private void DisableOtherAnimations(Animator animator, string animation)
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.name != animation)
            {
                animator.SetBool(parameter.name, false);
            }
        }
    }
}
