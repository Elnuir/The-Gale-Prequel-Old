using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NEnemyStateManager : MonoBehaviour
{
    // ORDER CHANGING CAUSES ANIMATIONS PROBLEMS
    public enum NEnemyState
    {
        Dying = 10,
        Hit = 20,
        MeleeAttack = 30,
        RangedAttack = 40,
        ComeCloser = 50,
        Patrol = 60,
        Idle = 70,
    }

    public float TargetDetectionDistance = 20;
    public float TargetDetectionDistanceY = 5;
    public float InitDelay = 0f;


    public NEnemyMovement EnemyMovement;
    public NEnemyAttack MeleeAttack;
    public NEnemyAttack RangedAttack;
    public NEnemyPatrol Patrol;
    public NEnemyHealth Health;
    public Text DebugOutput;


    private Animator _animator;
    private float _initDelay;
    private float _fixStateTimer; // Prevents changing state for a while

    private NEnemyState _currentState = NEnemyState.Idle;
    public NEnemyState CurrentState => _currentState;

    public Transform GetTarget()
    {
        var player = FindObjectOfType<Player>();

        if (player == null) return null;

        if (Vector2.Distance(player.transform.position, transform.position) > TargetDetectionDistance)
            return null;

        if (Mathf.Abs(player.transform.position.y - transform.position.y) > TargetDetectionDistanceY)
            return null;

        return player.transform;
    }

    private void DefineNewState()
    {
        var target = GetTarget();

        if (target == null)
        {
            if (EnemyMovement.CanMoveTo(Patrol.GetNextWaypoint()))
                SetCurrentState(NEnemyState.Patrol);
            else
                SetCurrentState(NEnemyState.Idle);
        }
        else
        {
            if (!EnemyMovement.CanMoveTo(target.position))
                SetCurrentState(NEnemyState.Idle);
            else if (MeleeAttack.CanAttack(target))
                SetCurrentState(NEnemyState.MeleeAttack, MeleeAttack.FixedTime);
            else if (RangedAttack.CanAttack(target))
                SetCurrentState(NEnemyState.RangedAttack, RangedAttack.FixedTime);
            else if (EnemyMovement.CanMoveToNow(target.position))
                SetCurrentState(NEnemyState.ComeCloser);
            else
                SetCurrentState(NEnemyState.Idle);
        }

        // Debug.Log(CurrentState);
    }

    private bool CanChangeState(NEnemyState newState)
    {
        return _fixStateTimer <= 0 || (int)newState <= (int)CurrentState;
    }

    private void DoStateAction(NEnemyState state)
    {
        switch (state)
        {
            case NEnemyState.Patrol:
                EnemyMovement.MoveTo(Patrol.GetNextWaypoint());
                break;
            case NEnemyState.ComeCloser:
                EnemyMovement.MoveTo(GetTarget().position);
                break;
            case NEnemyState.MeleeAttack:
                MeleeAttack.Attack(GetTarget());
                break;
            case NEnemyState.RangedAttack:
                RangedAttack.Attack(GetTarget());
                break;
        }
    }

    private void SetCurrentState(NEnemyState state, float fixedTime = 0f)
    {
        if (!CanChangeState(state)) return;

        if (state == NEnemyState.Idle)
        {
            var physics = GetComponent<Rigidbody2D>();
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, physics.velocity.y);
        }

        _currentState = state;
        
        _animator?.SetInteger("State", (int)state);
        _fixStateTimer = fixedTime;
        DoStateAction(state);
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        Health.OnHit += OnHit;
        Health.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        SetCurrentState(NEnemyState.Idle);
    }

    private void OnEnable()
    {
        SetCurrentState(NEnemyState.Idle);
        _initDelay = InitDelay;
    }

    private void OnHit()
    {
        SetCurrentState(NEnemyState.Hit, Health.HitTime);
    }

    private void OnDeath()
    {
        SetCurrentState(NEnemyState.Dying, Health.DyingTime);
    }

    private void Update()
    {
        _initDelay -= Time.deltaTime;

        if (_initDelay <= 0)
        {
            DefineNewState();
            if (_fixStateTimer >= 0f)
                _fixStateTimer -= Time.deltaTime;

            if (DebugOutput)
                DebugOutput.text = CurrentState.ToString();
        }
    }
}