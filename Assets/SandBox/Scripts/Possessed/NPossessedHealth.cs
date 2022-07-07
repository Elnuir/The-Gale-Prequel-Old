using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPossessedHealth : NEnemyHealth
{
    public UnityEvent Died;
    public UnityEvent KilledByPlayer;

    public float Health = 100;
    [HideInInspector] public bool isBelowExorcism;
    private float initHeath;

    private NJumpDamageReceiver jumpKillable;
    [SerializeField] private GameObject exorcismCheck, Fpossessed;

    [SerializeField] private float exorcismRadius;

    public float percantageOfUnding;
    public float percanteOfHealthToGiveForExorcism;
    [SerializeField] private LayerMask whatIsPlayer;
    public float knockBackForce;

    [SerializeField] public PossessedTutorialCheck possessedTutorialCheck;
    public bool IsAlive => Health > 0 || isBelowExorcism;

    private Throw throw1;
    private PlayerStats playerHealth;
    private HealthBar healthBar;
    private float lastDamageSourceX;

    private void Start()
    {
        jumpKillable = GetComponentInChildren<NJumpDamageReceiver>();
        initHeath = Health;
        throw1 = FindObjectOfType<Throw>();
        playerHealth = FindObjectOfType<PlayerStats>();
        healthBar = FindObjectOfType<HealthBar>();
    }

    // For calls via Message
    public void DamageReceive(float[] attackDetails)
    {
        DealDamage(attackDetails[0], attackDetails[1]);
        OnHit?.Invoke();
    }

    public void DealDamage(float amount, float sourceX)
    {
        float damageDirection;
        lastDamageSourceX = sourceX;

        if (IsAlive && !isBelowExorcism)
        {
            Health -= amount;

            if (sourceX > transform.position.x)
                damageDirection = -1;
            else
                damageDirection = 1;

            if (Health > 0.0f)
            {
                KnockBack(damageDirection);
            }
            else if (Health <= (initHeath / 100) * percantageOfUnding)
            {
                isBelowExorcism = true;
                if (possessedTutorialCheck.tutorIsDone == 0)
                {
                    possessedTutorialCheck.tutorBase.SetActive(true);
                }
            }
        }

    }

    void KnockBack(float damageDirection)
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * damageDirection * knockBackForce);
        //Debug.Log("set true");
    }

    void DieCheck()
    {
        if (isBelowExorcism)
        {
            Fpossessed.SetActive(true);
            jumpKillable.IsActive = false;
            Collider2D exorcismable =
                Physics2D.OverlapCircle(exorcismCheck.transform.position, exorcismRadius, whatIsPlayer);
            if (exorcismable)
            {
                if (Input.GetMouseButtonDown(1) && throw1.amountSaintWater == 0 && !GameManager.gameIsPaused)
                {
                    playerHealth.currentHealth = Mathf.Clamp(playerHealth.currentHealth - playerHealth.maxHelth / 100 * percanteOfHealthToGiveForExorcism, 1, playerHealth.maxHelth);
                    healthBar.SetHealth(playerHealth.currentHealth);
                    //isBelowExorcism = false;
                    Dead(Mathf.Abs(lastDamageSourceX) > 0);
                }

            }
        }
    }

    private void Update()
    {
        DieCheck();
    }

    public void Dead(bool killedByPlayer)
    {
        if (!IsAlive && !isBelowExorcism) return;

        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.EnemyKilledHandler();
        Fpossessed.SetActive(false);
        isBelowExorcism = false;
        //rigidbody2D.gravityScale = 3;
        gameObject.layer = 16;
        Invoke(nameof(AfterDeathPooling), 1.5f);
        OnDeath?.Invoke();
        Died?.Invoke();

        if (killedByPlayer)
            KilledByPlayer?.Invoke();
    }


    void AfterDeathPooling()
    {
        Health = initHeath;
        jumpKillable.IsActive = true;
        isBelowExorcism = false;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        //rigidbody2D.gravityScale = 0;
        gameObject.GetComponentInChildren<NJumpDamageReceiver>().IsActive = true;
        gameObject.PutToPool();
    }

    public override event Action OnHit;
    public override event Action OnDeath;
}
