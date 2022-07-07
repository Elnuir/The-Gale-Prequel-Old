using UnityEngine;
using UnityEngine.Events;

public class EntityHealthX : MonoBehaviour
{

    private bool IsAlive => Health > 0;
    public float Health = 100;
    [Range(0, 1f)] public float BlockAttackProbability = 0.5f;

    public UnityEvent Died;
    public UnityEvent Hit;
    public UnityEvent AttackBlocked;

    private void DealDamage(float amount, float sourceX)
    {
        Health -= amount;
        Hit?.Invoke();

        if (!IsAlive)
            Die();
    }

    private void BlockAttack()
    {
        AttackBlocked?.Invoke();
    }

    private bool CanBlockAttack()
    {
        return Random.Range(0f, 1f) < BlockAttackProbability;
    }

    void Die()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.EnemyKilledHandler();
        SetDamageReceiverLayer(16);
        Invoke(nameof(AfterDeathPooling), 1.5f);
        Died?.Invoke();
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

        if (CanBlockAttack())
            BlockAttack();
        else
            DealDamage(attackDetails[0], attackDetails[1]);
    }
}