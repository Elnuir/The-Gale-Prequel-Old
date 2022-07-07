using UnityEngine;
using UnityEngine.Events;

public class PeriodicDamage : MonoBehaviour
{
    public enum DamageTypes { Poison, Fire }

    public float Interval = 0.9f;
    public float Amount = 2f;
    public float Duration = 3f;
    public DamageTypes DamagerId;

    public UnityEvent Started;
    public UnityEvent Completed;

    private float _timer;
    private bool _completed;

    private void Update()
    {
        if ((_timer -= Time.deltaTime) <= 0)
            Complete();

        if (!_completed && ActionEx.CheckCooldown(Update, Interval))
            transform.SendMessage("Damage", GetAttackDetails());
    }

    public void Impose()
    {
        if (_completed)
            Started?.Invoke();

        _timer = Duration;
        _completed = false;
    }

    private void Complete()
    {
        _timer = 0;
        _completed = true;
        Completed?.Invoke();
    }

    private void OnDisable() => Complete();

    private float[] GetAttackDetails() => new[] { Amount, transform.position.x };
}
