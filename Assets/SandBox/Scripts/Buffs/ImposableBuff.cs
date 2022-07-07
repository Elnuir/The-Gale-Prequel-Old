using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ImposableBuff : MonoBehaviour
{
    public string BuffId;
    protected BuffManager _manager;

    protected virtual void Start()
    {
        _manager = FindObjectOfType<BuffManager>();

        if (_manager.CheckActive(BuffId))
            Impose();
    }

    protected abstract void Impose();
}
