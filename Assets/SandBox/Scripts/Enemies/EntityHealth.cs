using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    public float Health = 100;
    public bool IsInvincible;
    [Space] public UnityEvent Hit;
    public UnityEvent Death;
    public UnityEvent RealDeath;
    public BossHealthBar HealthBar;
    public GameObject Knife;
    public int SpheresAmount = 3;

    private bool isRealDead;

    private void Start()
    {
        HealthBar.SetHealth(Health);
    }

    void DamageReceive(float[] attackDetails)
    {
        if (IsInvincible) return;

        Health -= attackDetails[0];
        HealthBar.SetHealth(Health);

        if (Health > 0)
            Hit?.Invoke();
        else
            Death?.Invoke();

    }


    private void Update()
    {
        if (!isRealDead && SpheresAmount == 0)
        {
            RealDeath?.Invoke();
            isRealDead = true;

            HubWeaponOpenCheck.CheckIndexes();
            if (HubWeaponOpenCheck.isOpen[1] == 0)
                Knife.SetActive(true);
            else
                Invoke(nameof(DelayedWin), 2.5f);
        }
    }

    private void DelayedWin()
    {
        FindObjectOfType<GameManager>().RunWinCanvas();
    }
}