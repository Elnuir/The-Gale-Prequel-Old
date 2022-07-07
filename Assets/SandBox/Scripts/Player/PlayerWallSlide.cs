using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerWallSlide : MonoBehaviour
{
    private bool isTouchingFront;
    public Transform frontCheck;
    [FormerlySerializedAs("wallSliding")] public bool isHooked;
    //[SerializeField] float wallSlidingSpeed;
    [SerializeField] private float checkDistance;
    [SerializeField] private LayerMask whatIsGround;
    private Rigidbody2D rb;
    private Player player;

    public bool wallJumping;
    [SerializeField] float xWaffForce;
    [SerializeField] float yWaffForce;
    [SerializeField] private float reboundForceMultiplier, wallJumpTime;
    [SerializeField] private Vector2 jumpDirection;
    [SerializeField] public bool wallJumpPerformed; //FOR CHARACTER FLIPPING PURPOSE

    private float baseGravityScale;
    [SerializeField] private float baseWallHangingTime;
    private float leftWallHangingTime;
    private float moveInputY = 1f, moveInputX;
    [SerializeField] private float hookAgainTime;
  //  private float hookAgainTimeBase;
    [SerializeField] private bool canHook = true;
     

    //private bool wallJumping;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        baseGravityScale = rb.gravityScale;
        leftWallHangingTime = baseWallHangingTime;
    //    hookAgainTime = hookAgainTimeBase;
    }

    private void OnGUI()
    {
        //moveInputY = Mathf.Clamp(Input.GetAxisRaw("Vertical"), 0, 1);
        moveInputX = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && isHooked == true)
        {
            wallJumping = true;
            Invoke(nameof(SetWallJumpingToFalse), wallJumpTime);
        }
    }

    void Update()
    {

        // if (moveInputY == 0 && moveInputX == 0)
        //{
        //  jumpDirection = Vector2.up;
        //}
        //else
        jumpDirection = new Vector2(moveInputX, moveInputY);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        isTouchingFront = Physics2D.Raycast(frontCheck.position, transform.right, checkDistance, whatIsGround);
        if (isTouchingFront && !player.isGrounded && !player.isDead && canHook) //&& player.extraJumps > 0)
        {
            isHooked = true;
        }
        else
        {
            isHooked = false;
            rb.gravityScale = baseGravityScale;
            leftWallHangingTime = Mathf.Clamp(leftWallHangingTime += Time.deltaTime, 0, baseWallHangingTime);
        }

        if (isHooked && leftWallHangingTime >= 0)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            leftWallHangingTime -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.S))
            {
                leftWallHangingTime = 0;
                SetWallSlidingFalse();
            }
        }
        else
        {
            if(player.isGrounded)
            leftWallHangingTime = baseWallHangingTime;
            SetWallSlidingFalse();
        }

        

        if (wallJumping)
        {
            player.rb.velocity = Vector2.zero;
                rb.gravityScale = baseGravityScale;
                //player.Flip();
                //if(!player.facingRight)
                player.rb.AddForce(jumpDirection * reboundForceMultiplier * Time.fixedDeltaTime);
                //else
                  //  player.rb.AddForce(jumpDirection * reboundForceMultiplier * Time.fixedDeltaTime);
                  //player.rb.AddForce(new Vector2(-jumpDirection.x, jumpDirection.y) * reboundForceMultiplier * Time.fixedDeltaTime);
                wallJumpPerformed = true;
                canHook = false; Invoke(nameof(SetHookAgainTrue), hookAgainTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (wallJumping && LayerMask.LayerToName(other.gameObject.layer) == "Obstacle")
        {
            SetWallJumpingToFalse();
            SetWallSlidingFalse();
        }
    }

    void SetWallJumpingToFalse()
    {
        wallJumping = false;
        wallJumpPerformed = false;
    }

    void SetWallSlidingFalse()
    {
        isHooked = false;
        rb.gravityScale = baseGravityScale;
    }

    void SetHookAgainTrue()
    {
        canHook = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(frontCheck.transform.position, transform.right*checkDistance);
    }
}
