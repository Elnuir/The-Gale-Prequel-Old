using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NEnemyAttack : MonoBehaviour
{
    public  float FixedTime;
    public abstract bool CanAttack(Transform target);
    public abstract void Attack(Transform target);
}
