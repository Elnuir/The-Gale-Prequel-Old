using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMove : MonoBehaviour
{
    
    //private Animator animator;
    //private const string PLAYER_DASH = "CharacterDash";
    
    private Rigidbody2D rb;
    private Player player;
    private PlayerClimb playerClimb;
    public float dashSpeed;
    private float dashDuration;
    public float startdashDuration;
    public bool isDashing;
    private float speedBeforeDash;
    public ParticleSystem dashEffect;

    private bool LshiftHold, LshiftTap, Ahold, Atap, Dhold, Dtap;
   // private GameManager gameManager;
    
    
    void Start()
    {
        player = GetComponent<Player>();
        playerClimb = GetComponent<PlayerClimb>();
        rb = GetComponent<Rigidbody2D>();
        dashDuration = startdashDuration;
        //gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        LshiftHold = Input.GetKey(KeyCode.LeftShift);
        LshiftTap = Input.GetKeyDown(KeyCode.LeftShift);
        Ahold = Input.GetKey(KeyCode.A);
        Atap = Input.GetKeyDown(KeyCode.A);
        Dhold = Input.GetKey(KeyCode.D);
        Dtap = Input.GetKeyDown(KeyCode.D);
        if (isDashing == false)
        {
            //if(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
               // return;
            if ((LshiftTap && Ahold || LshiftTap && Dhold) && player.isRunning) //|| Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.D))
            {
                DashCommitment();
            }
            else if ((LshiftHold && Atap || LshiftHold && Dtap) && !player.isRunning)
            {
                DashCommitment();
            }
            
        }
        else
        {
            if (dashDuration <= 0 || playerClimb.isHooked)
            {
                gameObject.layer = 6;
                isDashing = false;
                dashDuration = startdashDuration;
                player.speed = speedBeforeDash;
                dashEffect.Stop();
            }
            else
            {
                dashDuration -= Time.deltaTime;
//                print("dashingEnding");
            }
        }
    }

    void DashCommitment()
    {
        if (!GameManager.gameIsPaused && !player.isDead && !playerClimb.isHooked)
        {
            gameObject.layer = 20;    //Makes player go thru enemies
            dashEffect.Play();
            isDashing = true;
            print("Dash");
            speedBeforeDash = player.speed;
            player.speed = dashSpeed;
            //player.ChangeAnimationState(PLAYER_DASH);
        }
    }
}
