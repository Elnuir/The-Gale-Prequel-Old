using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMouse : MonoBehaviour
{
    public Transform crosshair, ship;
    private float radius = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 center = ship.position;
        
        Vector2 direction = mousePosition - center; //direction from Center to Cursor
        Vector2 normalizedDirection = direction.normalized;
        crosshair.position = center + (normalizedDirection * radius);
    }
}
