using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ReacheableChecker : MonoBehaviour
{
    public abstract bool IsReacheable(Vector3 target);
}
