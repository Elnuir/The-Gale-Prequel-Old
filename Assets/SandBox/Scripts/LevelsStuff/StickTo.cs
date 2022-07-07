using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickTo : MonoBehaviour
{
    public Transform Target;
    public Vector2 Offset;

    private Rigidbody2D _physics;

    private void Start()
    {
        _physics = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Target)
        {
            _physics.position = (Vector2)Target.transform.position + Offset;
        }

    }
}
