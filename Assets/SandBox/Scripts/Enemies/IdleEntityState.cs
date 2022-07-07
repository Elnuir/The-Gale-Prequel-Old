using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleEntityState : AnimatedEntityState
{
    public override bool IsAvailable => enabled; 
}
