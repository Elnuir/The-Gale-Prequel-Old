using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BuffBase : MonoBehaviour
{
    public float ActiveTime;
    public Image TimeOutput;

    protected bool IsActive;
    private float timer;

    public virtual void Activate()
    {
        timer = ActiveTime;
        IsActive = true;
    }

    public virtual void Deactivate()
    {
        IsActive = false;
    }

    protected virtual void Update()
    {
        if (!IsActive) return;

        timer -= Time.deltaTime;

        if (TimeOutput != null)
            TimeOutput.fillAmount = timer / ActiveTime;

        if (timer <= 0)
            Deactivate();
    }
}
