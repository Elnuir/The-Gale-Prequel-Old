using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LowGravityJump : MonoBehaviour
{
    public float KeyDuration = 1f;
    public float GravityMultiplier = 0.5f;

    private float _initGravity;
    private float _durationTimer;
    private bool _isLowGravityMode;
    private Rigidbody2D _physics;

    private void Start()
    {
        _physics = GetComponent<Rigidbody2D>();
        _initGravity = _physics.gravityScale;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            _durationTimer += Time.deltaTime;
        else
            _durationTimer = 0;

        if (_durationTimer > KeyDuration && !_isLowGravityMode)
        {
            _physics.gravityScale = _initGravity * GravityMultiplier;
            _isLowGravityMode = true;
        }

        if (_durationTimer < KeyDuration && _isLowGravityMode)
        {
            _physics.gravityScale = _initGravity;
            _isLowGravityMode = false;
        }
    }
}
