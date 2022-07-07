using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTime : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private PossessedTutorialCheck possessedTutorialCheck;
    private void OnEnable()
    {
        Time.timeScale = 0;
        GameManager.gameIsPaused = true;
        gameManager.pauseDeactivated = true;
        possessedTutorialCheck = GetComponentInParent<PossessedTutorialCheck>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            possessedTutorialCheck.Next();
        }
    }
}