using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    [SerializeField] private Transform airLeftUp, airRightDown, hookLeftUp, hookRightDown, leftWallCheckLeftUp, leftWallCheckRightDown, rightWallCheckLeftUp, rightWallCheckRightDown;
    [SerializeField] LayerMask whatIsGround;
    private Rigidbody2D rb;
    private Player player;
    public bool isHooked;
    [SerializeField] private float hookDelay;
    private float hookDelayLeft;
    private bool canHookAgain = true;
    private bool canHookTimerGoes;
    private Collider2D air, hook, leftWall, rightWall;
    private float gravityScaleBase;
    
    //private RaycastHit2D rayLeft, rayRight;
    //[SerializeField] float rayLeftDistance, rayRightDistance;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        hookDelayLeft = hookDelay;
        gravityScaleBase = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        canHookTimer();
        Hooked();
        if (!isHooked && canHookAgain && !player.isGrounded && !player.isAttacking && !player.isDead)
        {
            air = Physics2D.OverlapArea(airLeftUp.position, airRightDown.position, whatIsGround);
            hook = Physics2D.OverlapArea(hookLeftUp.position, hookRightDown.position, whatIsGround);
            leftWall = Physics2D.OverlapArea(leftWallCheckLeftUp.position, leftWallCheckRightDown.position, whatIsGround);
            rightWall = Physics2D.OverlapArea(rightWallCheckLeftUp.position, rightWallCheckRightDown.position, whatIsGround);
            if (!air && hook && rightWall && !leftWall && rb.velocity.y >= -20f && !player.isDead)
            {
                isHooked = true;
                
            }
        }
        //print((bool)rayRight + "     " + (bool)rayLeft);
    }

    void Hooked()
    {
        if (isHooked && (player.isHitted || Input.GetButtonDown("Jump") || player.isDead || player.isGrounded || Input.GetKeyDown(KeyCode.S)))
        {
           // rb.simulated = true;
            isHooked = false;
            canHookAgain = false;
            canHookTimerGoes = true;
            rb.gravityScale = gravityScaleBase;

        }
    }

    void canHookTimer()
    {
        if (canHookTimerGoes)
        {
            hookDelayLeft -= Time.deltaTime;
            if (hookDelayLeft <= 0)
            {
                canHookTimerGoes = false;
                canHookAgain = true;
                hookDelayLeft = hookDelay;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
            new Vector2((airLeftUp.position.x + airRightDown.position.x) / 2, (airLeftUp.position.y + airRightDown.position.y) / 2),
            new Vector2((airRightDown.position.x - airLeftUp.position.x), (airLeftUp.position.y - airRightDown.position.y)));
        Gizmos.DrawWireCube(
            new Vector2((hookLeftUp.position.x + hookRightDown.position.x) / 2, (hookLeftUp.position.y + hookRightDown.position.y) / 2),
            new Vector2((hookRightDown.position.x - hookLeftUp.position.x), (hookRightDown.position.y - hookLeftUp.position.y)));
        Gizmos.DrawWireCube(
            new Vector2((leftWallCheckLeftUp.position.x + leftWallCheckRightDown.position.x) / 2, (leftWallCheckLeftUp.position.y + leftWallCheckRightDown.position.y) / 2),
            new Vector2((leftWallCheckRightDown.position.x - leftWallCheckLeftUp.position.x), (leftWallCheckRightDown.position.y - leftWallCheckLeftUp.position.y)));
        Gizmos.DrawWireCube(
            new Vector2((rightWallCheckLeftUp.position.x + rightWallCheckRightDown.position.x) / 2, (rightWallCheckLeftUp.position.y + rightWallCheckRightDown.position.y) / 2),
            new Vector2((rightWallCheckRightDown.position.x - rightWallCheckLeftUp.position.x), (rightWallCheckRightDown.position.y - rightWallCheckLeftUp.position.y)));

    }
}
