using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class NEnemyMovement : MonoBehaviour
{
    public float Speed = 5;
    public abstract bool CanMoveTo(Vector3 position);
    public abstract bool CanMoveToNow(Vector3 position);
    public abstract void MoveTo(Vector3 position);
}
