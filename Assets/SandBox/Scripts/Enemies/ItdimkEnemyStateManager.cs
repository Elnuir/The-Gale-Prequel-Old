using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Pathfinding;
using UnityEngine;

public class ItdimkEnemyStateManager : MonoBehaviour
{
    public enum NibblerState
    {
        Dying,
        Hit,
        Patrol,
        Idle,
        MeleeAttack,
        RangedAttack,
        ComeCloser,
    }

    public float TargetDetectionDistance = 20;
    public float MeleeAttackRange = 5;
    public float RangedAttackRange = 20;
    public float RangedAttackDeltaY = 2;

    public event Action<Path> OnPathComplete;
    
    public float Health = 100;
    private float _fixStateTimer; // Prevents changing state for a while
    private Animator _animator;
    private Seeker _seeker;
    private NibblerState _currentState = NibblerState.Idle;


    public Path TargetPath;

    public NibblerState CurrentState
    {
        get => _currentState;
        private set
        {
            _currentState = value;
            _animator.SetInteger("State", (int) value);
        }
    }

    private bool CanChangeState => _fixStateTimer <= 0f;


    [CanBeNull]
    public Transform GetTarget()
    {
        var player = FindObjectOfType<Player>();

        if (player == null || player.isDead) return null;

        if (Vector2.Distance(player.transform.position, transform.position) > TargetDetectionDistance)
            return null;

        return player.transform;
    }

    private bool IsTargetReachable(Transform target)
    {
        if (TargetPath.vectorPath.Count < 2) return true;

        int maxCount = 0;
        int count = 0;
        for (int i = 1; i < TargetPath.vectorPath.Count; ++i)
        {
            var delta = TargetPath.vectorPath[i].y - TargetPath.vectorPath[i - 1].y;
            var deltaX = TargetPath.vectorPath[i].x - TargetPath.vectorPath[i - 1].x;
            var angle = Mathf.Atan2(delta, deltaX) * Mathf.Rad2Deg;
            if (angle > 90-30 && angle < 90+30)
                count++;
            else
            {
                if (count > maxCount)
                    maxCount = count;
                count = 0;
            }
        }

        if (count > maxCount)
            maxCount = count;

        return maxCount <= 3;
    }

    private bool IsMeleeAttackAvailable(Transform target)
    {
        if (!((Func<Transform, bool>) IsMeleeAttackAvailable).CheckCooldown(1f)) return false;

        if (Vector2.Distance(target.transform.position, transform.position) <= MeleeAttackRange)
        {
            FixStateFor(0.5f);
            return true;
        }

        return false;
    }

    private bool IsRangedAttackAvailable(Transform target)
    {
        if (!((Func<Transform, bool>) IsRangedAttackAvailable).CheckCooldown(1f)) return false;

        float distanceToTarget = Vector2.Distance(target.transform.position, transform.position);

        bool distance = distanceToTarget > MeleeAttackRange && distanceToTarget <= RangedAttackRange;
        bool deltaY = Mathf.Abs(target.transform.position.y - transform.position.y) <= RangedAttackDeltaY;

        if (distance && deltaY)
        {
            FixStateFor(0.3f);
            return true;
        }

        return false;
    }

    private bool IsCloseEnough(Transform target)
    {
        return Vector2.Distance(target.transform.position, transform.position) < MeleeAttackRange;
    }
    
    private void DefineNewState()
    {
        if (!CanChangeState) return;
        
        var target = GetTarget();

        if (target == null)
            CurrentState = NibblerState.Patrol;
        else
        {
            if (!IsTargetReachable(target))
                CurrentState = NibblerState.Idle;
            else if (IsMeleeAttackAvailable(target))
                CurrentState = NibblerState.MeleeAttack;
            else if (IsRangedAttackAvailable(target))
                CurrentState = NibblerState.RangedAttack;
            else if (!IsCloseEnough(target))
                CurrentState = NibblerState.ComeCloser;
            else
                CurrentState = NibblerState.Idle;
        }
    }

    
    private void FixStateFor(float seconds)
    {
        _fixStateTimer = seconds;
    }


    // For calls via Message
    public void DamageReceive(float[] attackDetails)
    {
        
        Health -= attackDetails[0];
        if (Health > 0)
        {
            CurrentState = NibblerState.Hit;
        FixStateFor(0.3f);
        }
        else
        {
            CurrentState = NibblerState.Dying;
           Dead(); 
           FixStateFor(2f);
        }

    }

    void Dead()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.EnemyKilledHandler();
        gameObject.layer = 16;
        Invoke(nameof(AfterDeathPooling), 1.5f);
    }
    
    void AfterDeathPooling()
    {
        gameObject.layer = 11;
        gameObject.PutToPool();
    }
    
    private void Start()
    {
        _seeker = GetComponent<Seeker>();
        _animator = GetComponent<Animator>();
        InvokeRepeating("UpdatePath", 0f, 1.4f);

        this.OnPathComplete += (p) =>
        {
            TargetPath = p;
        };
    }

    private void Update()
    {
        DefineNewState();
        ;
        Debug.Log(CurrentState.ToString());
        if (_fixStateTimer >= 0f)
            _fixStateTimer -= Time.deltaTime;
    }

    void UpdatePath()
    {
        if (_seeker.IsDone())
            _seeker.StartPath(transform.position, GetTarget().transform.position, (p) => OnPathComplete?.Invoke(p));
    }

}