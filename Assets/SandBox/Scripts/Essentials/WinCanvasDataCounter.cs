using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinCanvasDataCounter : MonoBehaviour
{
    [SerializeField] private Text killedEnemies, collectedCoins;
    [SerializeField] private AchievementsManager achievementsManager;
    [SerializeField] private Score score;
    private string killedEnemiesPattern = "Killed Enemies: ";
    private string collectedCoinsPattern = "x";

    private void OnEnable()
    {
        killedEnemies.text = killedEnemiesPattern + achievementsManager.killedEnemiesOnTheLevel;
        collectedCoins.text = collectedCoinsPattern + score.ShowFinalCoinsDifference();
        Time.timeScale = 0;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
