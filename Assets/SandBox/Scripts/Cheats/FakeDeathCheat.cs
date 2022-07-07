using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeDeathCheat : CheatBase
{
    private Player _player;

    protected override void ActivateCheat()
    {
        base.ActivateCheat();
        _player = FindObjectOfType<Player>();
        _player.gameObject.SetActive(false);
    }

    protected override void DeactivateCheat()
    {
        base.DeactivateCheat();
        _player?.gameObject.SetActive(true);
    }
}
