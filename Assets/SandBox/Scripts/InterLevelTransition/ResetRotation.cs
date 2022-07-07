using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRotation : MonoBehaviour
{
    public void Do()
    {
        transform.rotation = Quaternion.identity;
    }
}
