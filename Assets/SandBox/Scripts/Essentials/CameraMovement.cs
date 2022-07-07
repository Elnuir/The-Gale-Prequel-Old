using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    void Update()
    {
        //TODO: clamp this shit
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y + 100, -958, 958), transform.position.z);
            print("up");
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0) // backwards
        {
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y - 100, -958, 958), transform.position.z);
            print("down");
        }
    }
}
