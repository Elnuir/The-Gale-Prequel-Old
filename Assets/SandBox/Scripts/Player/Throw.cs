using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Throw : MonoBehaviour
{
    private Vector2 direction, mousePosition, center, normalizedDirection;


    public GameObject knife;
    public GameObject saintWater;
    public GameObject grenade;

    public float launchForceKnife, radiusOfPointer;
    public float launchForceSaintWater, launchForceGrenade;

    public Transform shotPoint;

    public int baseAmountOfKnives;
    public int amountOfKnives;
    private KnivesCounter knivesCounter;

    public int amountSaintWater;
    private SaintWaterCounter saintWaterCounter;

    private Player player;
    private Transform playerTransform;
    private PlayerClimb playerClimb;
    public float attackDamageKnife;
    public float baseAttackDamageKnife;
    public float attackDamageGrenage;
    public bool canBeUsedKnives, canBeUsedWater, canBeUsedGrenade;
    private SpriteRenderer pointerKnife;
    private Animator animator;

    private float yMousePlayerDifference, xMousePlayerDifference;
    private float angle;
    


    //public GameObject point;
    //  private GameObject[] points;
    //public int numberOfPoints;
    // public float spaceBetweenPoints;
    void Start()
    {
        animator = GetComponentInParent<Animator>();
        amountOfKnives = baseAmountOfKnives;
        baseAttackDamageKnife = attackDamageKnife;
        //  points = new GameObject[numberOfPoints];
        // for (int i = 0; i < numberOfPoints; i++)
        // {
        //     points[i] = Instantiate(point, shotPoint.position, Quaternion.identity);
        // }
        knivesCounter = FindObjectOfType<KnivesCounter>();
        saintWaterCounter = FindObjectOfType<SaintWaterCounter>();
        player = GetComponentInParent<Player>();
        playerClimb = GetComponentInParent<PlayerClimb>();
        pointerKnife = GetComponent<SpriteRenderer>();
        playerTransform = player.gameObject.GetComponent<Transform>();
        pointerKnife.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Vector2 bowPosition = transform.position;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        center = player.gameObject.transform.position;
        direction = mousePosition - center;
        normalizedDirection = direction.normalized;
        transform.position = center + (normalizedDirection * radiusOfPointer);
        //direction = mousePosition - bowPosition;
        float dY = mousePosition.y - transform.parent.position.y;
        float dX = mousePosition.x - transform.parent.position.x;
        angle = Mathf.Atan2(dY, dX) * Mathf.Rad2Deg;
//        animator.SetFloat("AngleToThrow", angle);
        // print(angle);
        InputCheck();
        //if (!transform.parent.GetComponent<Player>().facingRight)
        //{
        //  direction *= -1;
        // }
        transform.right = direction;
        
            if (Input.GetMouseButtonUp(0) && !player.isDead && !playerClimb.isHooked && canBeUsedKnives)
            {
                if (!GameManager.gameIsPaused)
                {
                    if (amountOfKnives > 0)
                    {
                        DirectionCheckKnife();
                    }
                }
            }

            if (Input.GetMouseButtonUp(1) && !player.isDead && !playerClimb.isHooked && canBeUsedWater)
            {
                if (!GameManager.gameIsPaused)
                {
                    if (amountSaintWater > 0)
                    {
                        DirectionCheckHolyWater();
                        
                    }
                }
            }
            // if (Input.GetKeyUp(KeyCode.J) && !player.isDead && !playerClimb.isHooked && canBeUsedWater)
            // {
            //     if (!GameManager.gameIsPaused)
            //     {
            //         if (amountSaintWater > 0)
            //         {
            //             DirectionCheckHolyWater();
            //             
            //         }
            //     }
            // }
    }

    void InputCheck()
    {
        
        if (Input.GetMouseButton(0) && canBeUsedKnives && amountOfKnives > 0  && !player.isDead && !playerClimb.isHooked)
        {
            pointerKnife.enabled = true;
        }

        else if (Input.GetMouseButton(1) && canBeUsedWater && amountSaintWater > 0  && !player.isDead && !playerClimb.isHooked)
        {
            pointerKnife.enabled = true;
        }
        else
        {
            pointerKnife.enabled = false;
        }
    }

    public void ShootSaintWater()
    {
        amountSaintWater--;
        saintWaterCounter.amount = amountSaintWater;
        saintWaterCounter.TextUpdate();
        GameObject newArrow = Instantiate(saintWater, shotPoint.position, shotPoint.rotation);
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForceSaintWater;
    }
    public void ShootGrenade()
    {
        //amountSaintWater--;
       // saintWaterCounter.amount = amountSaintWater;
       // saintWaterCounter.TextUpdate();
        GameObject newArrow = Instantiate(grenade, shotPoint.position, shotPoint.rotation);
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForceGrenade;
    }

    public void ShootKnife()
    {
        amountOfKnives--;
        knivesCounter.amount = amountOfKnives;
        knivesCounter.TextUpdate();
        GameObject newArrow = Instantiate(knife, shotPoint.position, shotPoint.rotation);
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForceKnife;
    }

    void DirectionCheckKnife()
    {
        animator.SetBool("isThrowing", true);
        if (player.facingRight)
        {
            if (angle < 90f && angle > 45f)
            {
                animator.Play("ThrowAKnifeUp");
            }
            else if (angle < 45f && angle > -45f)
            {
                animator.Play("ThrowAKnifeRight");
            }
            else if (angle < -45f && angle > -90)
            {
                animator.Play("ThrowAKnifeDown");
            }
            else if (angle < -90 && angle > -135)
            {
                player.Flip();
                animator.Play("ThrowAKnifeDown");
            }
            else if (angle < 135f && angle > 90f)
            {
                player.Flip();
                animator.Play("ThrowAKnifeUp");
            }
            else
            {
                player.Flip();
                animator.Play("ThrowAKnifeRight");
            }
        }
        if (!player.facingRight)
        {
            if (angle < 90f && angle > 45f)
            {
                player.Flip();
                animator.Play("ThrowAKnifeUp");
            }
            else if (angle < 45f && angle > -45f)
            {
                player.Flip();
                animator.Play("ThrowAKnifeRight");
            }
            else if (angle < -45f && angle > -90)
            {
                player.Flip();
                animator.Play("ThrowAKnifeDown");
            }
            else if (angle < -90 && angle > -135)
            {
                animator.Play("ThrowAKnifeDown");
            }
            else if (angle < 135f && angle > 90f)
            {
                animator.Play("ThrowAKnifeUp");
            }
            else
            {
                animator.Play("ThrowAKnifeRight");
            }
        }

        player.flipRestricted = true;
    }
    public void DirectionCheckHolyWater()
    {
        animator.SetBool("isThrowing", true);
        print("dsdsdd");
        if (player.facingRight)
        {
            if (angle < 90f && angle > 45f)
            {
                animator.Play("ThrowAHolyWaterUp");
            }
            else if (angle < 45f && angle > -45f)
            {
                animator.Play("ThrowAHolyWaterRight");
            }
            else if (angle < -45f && angle > -90)
            {
                animator.Play("ThrowAHolyWaterDown");
            }
            else if (angle < -90 && angle > -135)
            {
                player.Flip();
                animator.Play("ThrowAHolyWaterDown");
            }
            else if (angle < 135f && angle > 90f)
            {
                player.Flip();
                animator.Play("ThrowAHolyWaterUp");
            }
            else
            {
                player.Flip();
                animator.Play("ThrowAHolyWaterRight");
            }
        }
        if (!player.facingRight)
        {
            if (angle < 90f && angle > 45f)
            {
                player.Flip();
                animator.Play("ThrowAHolyWaterUp");
            }
            else if (angle < 45f && angle > -45f)
            {
                player.Flip();
                animator.Play("ThrowAHolyWaterRight");
            }
            else if (angle < -45f && angle > -90)
            {
                player.Flip();
                animator.Play("ThrowAHolyWaterDown");
            }
            else if (angle < -90 && angle > -135)
            {
                animator.Play("ThrowAHolyWaterDown");
            }
            else if (angle < 135f && angle > 90f)
            {
                animator.Play("ThrowAHolyWaterUp");
            }
            else
            {
                animator.Play("ThrowAHolyWaterRight");
            }
        }

        player.flipRestricted = true;
    }
}

// Vector2 PointPosition(float t)
//{
//    Vector2 position = (Vector2) shotPoint.position + (direction.normalized * launchForce * t) + 0.5f * Physics2D.gravity * (t * t);
//   return position;
//}