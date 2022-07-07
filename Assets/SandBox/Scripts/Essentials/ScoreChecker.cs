using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreChecker : MonoBehaviour
{
    private int currentMenuScore = 0;
    private string score = "Coins: ";
    private Text scoreText;
    void Start()
    {
        scoreText = GetComponent<Text>();
        scoreText.text = score + PlayerPrefs.GetInt("full-score");         //Set score after level passed
        //  PlayerPrefs.GetInt("full-score");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
