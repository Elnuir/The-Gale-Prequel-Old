using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureManager : MonoBehaviour
{
    private Score score;
    private SettingsManager settingsManager;
    private GameManager gameManager;

    void Start()
    {
        score = FindObjectOfType<Score>();
        settingsManager = FindObjectOfType<SettingsManager>();
        gameManager = FindObjectOfType<GameManager>();
    }
    public void GetTheTreasure()
    {
        score.currentScore += 700;
        gameManager.RunWinCanvas();
        //settingsManager.Save();
    }
}
