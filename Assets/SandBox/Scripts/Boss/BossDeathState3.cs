using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathState3 : AnimatedEntityState
{
    public float TeleportTimeOffset = 0.5f;
    public Transform IdleSpot;
    public Transform BodyToTeleport;


    private float _teleportTimer;

    public override void MakeAvailable()
    {
        base.MakeAvailable();
        _teleportTimer = Duration * TeleportTimeOffset;
    }

    private bool ShouldTeleport()
    {
        return IsActive && Vector2.Distance(transform.position, IdleSpot.position) > 0.5f && _teleportTimer <= 0;
    }

    private void Teleport()
    {
        BodyToTeleport.transform.position = IdleSpot.position;
    }

    protected override void Update()
    {
        base.Update();
        if (_teleportTimer >= 0)
            _teleportTimer -= Time.deltaTime;

        if (ShouldTeleport())
            Teleport();

    }
}
