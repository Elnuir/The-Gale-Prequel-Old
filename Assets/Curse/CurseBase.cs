using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CurseBase : MonoBehaviour
{
    public string Id;
    public abstract bool CanApply(Player p);
    public abstract void Apply(Player p);
}
