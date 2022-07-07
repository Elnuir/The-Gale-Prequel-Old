using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class NBasicEnemyHealth : NEnemyHealth
{

    private bool IsAlive => Health > 0;
    public float Health = 100;

    public UnityEvent Died;
    public UnityEvent KilledByUnknown;
    public UnityEvent KilledByPlayer;

    public void DealDamage(float amount, float sourceX)
    {
        Health -= amount;

        if (!IsAlive)
            Die(Mathf.Abs(sourceX) > 0);
    }

    void Die(bool killedByPlayer)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.EnemyKilledHandler();
        SetDamageReceiverLayer(16);
        Invoke(nameof(AfterDeathPooling), 1.5f);
        OnDeath?.Invoke();

        Died?.Invoke();

        if (killedByPlayer)
            KilledByPlayer?.Invoke();
        else
            KilledByUnknown?.Invoke();

    }

    void SetDamageReceiverLayer(int layer)
    {
        var receiver = GetComponentInChildren<NDamageReciever>();
        receiver.gameObject.layer = layer;
    }

    void AfterDeathPooling()
    {
        SetDamageReceiverLayer(12);
        Health = 100;
        gameObject.PutToPool();
        gameObject.GetComponentInChildren<NJumpDamageReceiver>().IsActive = true;
    }

    // For calls via Message
    public void DamageReceive(float[] attackDetails)
    {
        if (!IsAlive) return;
        DealDamage(attackDetails[0], attackDetails[1]);
        OnHit?.Invoke();
    }

    public override event Action OnHit;
    public override event Action OnDeath;
}