using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCheat : CheatBase
{
    public int ExtraJumps = 100;

    private Player _player;
    private int _initExtraJumps;
    private int _initExtraJumpsValue;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _initExtraJumpsValue = _player.extraJumpsValue;
        _initExtraJumps = _player.extraJumps;
    }

    protected override void ActivateCheat()
    {
        base.ActivateCheat();
        _player.extraJumpsValue = ExtraJumps;
        _player.extraJumps = ExtraJumps;
    }

    protected override void DeactivateCheat()
    {
        base.DeactivateCheat();
        _player.extraJumps = _initExtraJumps;
        _player.extraJumpsValue = _initExtraJumpsValue;
    }
}
