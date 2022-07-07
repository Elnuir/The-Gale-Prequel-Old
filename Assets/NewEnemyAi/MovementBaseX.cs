using System.Collections.Specialized;
using UnityEngine;

public abstract class MovementBaseX : MonoBehaviour
{
    public abstract bool IsMoving { get; }
    public abstract void MoveTo(Vector3 target);
    public abstract void StopMovement();
}