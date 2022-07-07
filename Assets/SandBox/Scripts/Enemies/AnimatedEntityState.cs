using UnityEngine;

public class AnimatedEntityState : AnimatedEntityStateBase
{
    public override bool IsAvailable => enabled && StateLockTime > 0;

    protected float StateLockTime;

    public override void ActivateState()
    {
        base.ActivateState();
        Animator.Play(AnimationName);
    }

    public override void DeactivateState()
    {
        base.DeactivateState();
        StateLockTime = 0;
    }

    public virtual void MakeAvailable()
    {
        StateLockTime = Duration;
    }

    protected virtual void Update()
    {
        if (IsActive && StateLockTime > 0)
            StateLockTime -= Time.deltaTime;
    }
}