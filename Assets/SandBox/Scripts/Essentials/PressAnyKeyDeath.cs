using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyKeyDeath : MonoBehaviour
{
    private Score score;

    private void Start()
    {
        score = FindObjectOfType<Score>();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            score.Death();
        }
    }
}
