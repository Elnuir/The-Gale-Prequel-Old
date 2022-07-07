using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Score : MonoBehaviour
{

    
    
    public int currentScore;
    private int scoreAfterLoading;
    
    private string score = "x";

    private Text _textWindow;
    private Text textWindow => _textWindow ? _textWindow : _textWindow = GetComponent<Text>();
    

    private void Update()
    {

    }

    private void Start()
    {
        scoreAfterLoading = currentScore;
    }

    public void UpdateScore()
    {
        textWindow.text = score + currentScore;
    }

    public int ShowFinalCoinsDifference()
    {
        return currentScore - scoreAfterLoading;
    }
    public void Death()
    {
        AudioSettings audioSettings = FindObjectOfType<AudioSettings>();
        float a = audioSettings.MusicVolume;
        float b = audioSettings.EffectsVolume;
        PlayerPrefs.DeleteAll();
        audioSettings.MusicVolume = a;
        audioSettings.EffectsVolume = b;
        MapCanvasEnabler.isEnabledMapCanvas = false;
        SceneManager.LoadScene("MainMenu");
    }
}
