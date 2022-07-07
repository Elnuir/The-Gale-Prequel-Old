using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavingEdgeJump : MonoBehaviour
{
    Player _player;
    Rigidbody2D _physics;
    float _canJumpTimer;
    bool _isGroundedPrevious;
    Vector2 _prevVelocity;

    public float LeavingEdgeJumpTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Player>();
        _physics = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _prevVelocity.y < 0 && _canJumpTimer > 0)
        {
            _canJumpTimer = 0;
            _player.extraJumps ++;
        }


        if (_isGroundedPrevious && !_player.isGrounded)
            _canJumpTimer = LeavingEdgeJumpTime;

        if(_physics.velocity.y > 0)
            _canJumpTimer = 0;

        _canJumpTimer -= Time.deltaTime;
        _isGroundedPrevious = _player.isGrounded;
        _prevVelocity = _physics.velocity;
    }
}
