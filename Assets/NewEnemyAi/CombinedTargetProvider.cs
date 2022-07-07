using System.Linq;
using UnityEngine;

public class CombinedTargetProvider : TargetProviderBase
{
    public TargetProviderBase[] TargetProviders;

    private void Start()
    {
        if (TargetProviders == null || TargetProviders.Length == 0)
            TargetProviders = GetComponentsInChildren<TargetProviderBase>().Where(p => p != this).ToArray();
    }

    public override Transform GetTarget()
    {
        var prodiver = TargetProviders.FirstOrDefault(t => t.GetTarget() != null);
        return prodiver?.GetTarget();
    }
}