using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetProvider : TargetProviderBase
{
    public string PlayerTag = "Player";
    public float MaxDistance = 15f;

    private GameObject _player;

    public override Transform GetTarget()
    {
        if (_player == null)
            return null;

        if (Vector2.Distance(transform.position, _player.transform.position) <= MaxDistance)
            return _player?.transform;
        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag(PlayerTag);
    }
}
