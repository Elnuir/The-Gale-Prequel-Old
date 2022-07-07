using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessedTutorialCheck : MonoBehaviour
{
    public int tutorIsDone = 0;
    public GameObject tutorBase;
    public GameObject[] texts;
    private int currentText = 0;
    [SerializeField]private GameManager gameManager;

    public void Next()
    {
        print(currentText);
        
        texts[currentText].SetActive(false);
        currentText++;
        if (currentText > texts.Length - 1)
        {
            tutorBase.SetActive(false);
            tutorIsDone = 1;
            Time.timeScale = 1;
            GameManager.gameIsPaused = false;
            gameManager.pauseDeactivated = false;
        }
        texts[currentText].SetActive(true);
    }

}
