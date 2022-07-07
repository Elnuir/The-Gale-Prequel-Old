using UnityEngine;
using UnityEngine.Events;

public abstract class EntityState : MonoBehaviour
{
    public int Priority = 0;
    public abstract bool IsAvailable { get; }
    public bool IsActive { get; private set; }
    
    public virtual void ActivateState()
    {
        IsActive = true;
    }

    public virtual void DeactivateState()
    {
        IsActive = false;
    }
}