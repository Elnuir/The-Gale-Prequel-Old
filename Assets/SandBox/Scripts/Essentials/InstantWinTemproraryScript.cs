using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class InstantWinTemproraryScript : MonoBehaviour
{
    private LevelController levelController;
    public UnityEvent LevelCompleted;
    
    void Start()
    {
        levelController = FindObjectOfType<LevelController>();
    }
    public void Win()
    {
        LevelCompleted?.Invoke();
        levelController.CompleteLevel(CurrentLevel.CurrLevel);
        SceneManager.LoadScene("MainMenu");
    }
}
