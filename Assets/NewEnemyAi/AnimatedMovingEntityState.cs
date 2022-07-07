using UnityEngine;

public class AnimatedMovingEntityState : AnimatedEntityState
{
    public string IdleAnimationName;
    public MovementBaseX Movement;
    public ReacheableChecker ReacheableChecker;
    public TargetProviderBase TargetProvider;
    public override bool IsAvailable => true;
    private bool _isPlayingIdle;

    protected override void Start()
    {
        base.Start();
        TargetProvider ??= GetComponent<TargetProviderBase>();
        Movement ??= GetComponent<MovementBaseX>();
        ReacheableChecker ??= GetComponent<ReacheableChecker>();
    }

    protected override void Update()
    {
        if (!IsActive) return;

        if (!_isPlayingIdle && !Movement.IsMoving)
        {
            _isPlayingIdle = true;
            Animator.Play(IdleAnimationName);
        }

        if (_isPlayingIdle && Movement.IsMoving)
        {
            _isPlayingIdle = false;
            Animator.Play(AnimationName);
        }

        var target = TargetProvider.GetTarget();

        if (target != null)
            Movement.MoveTo(target.position);
    }

    public override void DeactivateState()
    {
        base.DeactivateState();
        Movement.StopMovement();
    }
}