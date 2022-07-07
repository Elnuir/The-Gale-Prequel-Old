using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCanvasCrutch : MonoBehaviour
{
    public Transform flipMarker;
    private bool isFacingRight;
    public bool TryIfDoesntWork;

    private void Start()
    {
        isFacingRight = Mathf.Abs(flipMarker.transform.eulerAngles.y) < 90;
        if (TryIfDoesntWork)
            isFacingRight = !isFacingRight;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFacingRight && Mathf.Abs(flipMarker.rotation.eulerAngles.y) > 90)
        {
            transform.Rotate(0, 180, 0);
            isFacingRight = false;
        }
        else if (!isFacingRight && Mathf.Abs(flipMarker.rotation.eulerAngles.y) < 90)
        {
            transform.Rotate(0, -180, 0);
            isFacingRight = true;
        }

    }
}
