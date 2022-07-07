using UnityEngine;

public class SimpleTargetProvider : TargetProviderBase
{
    public GameObject Target;
    

    public override Transform GetTarget()
    {
        return Target.transform;
    }
}
