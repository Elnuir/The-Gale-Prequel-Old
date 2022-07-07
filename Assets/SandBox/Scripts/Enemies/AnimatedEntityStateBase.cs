using System.Linq;
using UnityEngine;

public abstract class AnimatedEntityStateBase : EntityState
{
    public string AnimationName;
    protected Animator Animator;

    protected float Duration
    {
        get
        {
            if (Animator == null) return 0;
            var clip = Animator.runtimeAnimatorController.animationClips.FirstOrDefault(c => c.name == AnimationName);
            if (clip == null) return 0;
            return clip.length;
        }
    }

    protected virtual void Start()
    {
        Animator = GetComponentInChildren<Animator>();
    }
}