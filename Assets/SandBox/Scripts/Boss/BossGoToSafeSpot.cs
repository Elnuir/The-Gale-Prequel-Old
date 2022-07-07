using System.Linq;
using UnityEngine;

public class BossGoToSafeSpot : AnimatedEntityState
{
    public Transform[] SafeSpots;
    [HideInInspector] public Transform CurrentSpot;

    public EntityHealth Health;
    public TargetProviderBase Target;
    private Rigidbody2D _physics;

    public float TargetNearbyDistance = 6;
    public float AttempsBeforeRespawn = 2f;
    public float BaseDurationMultiplier = 0.5f;

    private float changeSafeSpotTimer = 0;

    public override bool IsAvailable
    {
        get
        {
            if (IsActive && CurrentSpot && Vector2.Distance(CurrentSpot.position, Target.GetTarget().position) < TargetNearbyDistance)
                return true;
            return base.IsAvailable;
        }
    }

    protected override void Start()
    {
        base.Start();
        _physics = GetComponent<Rigidbody2D>();
    }

    public override void ActivateState()
    {
        base.ActivateState();
        SelectCurrentSpot();
        changeSafeSpotTimer = 0;
        Health.IsInvincible = true;
    }

    public override void MakeAvailable()
    {
        base.MakeAvailable();
        StateLockTime = Duration * BaseDurationMultiplier;
    }

    public override void DeactivateState()
    {
        if (!IsAvailable)
            base.DeactivateState();
        _physics.velocity = Vector2.zero;
        Health.IsInvincible = false;
    }
    protected override void Update()
    {
        base.Update();

        if (IsActive)
        {
            if (changeSafeSpotTimer > 0)
                changeSafeSpotTimer -= Time.deltaTime;
            else
                SelectCurrentSpot();
        }
    }

    private void SelectCurrentSpot()
    {
        CurrentSpot = SafeSpots.Where(s => s != CurrentSpot).ElementAt(Random.Range(0, SafeSpots.Length - 1));
        transform.position = CurrentSpot.position;
        transform.rotation = Quaternion.identity;
        changeSafeSpotTimer = Duration * AttempsBeforeRespawn;
        StateLockTime = Duration * BaseDurationMultiplier;
        Animator.Play(AnimationName);
    }
}