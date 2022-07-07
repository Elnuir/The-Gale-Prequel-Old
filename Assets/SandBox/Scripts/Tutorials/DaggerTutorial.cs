using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DaggerTutorial : MonoBehaviour
{
    private int currentsection;
    Animator animator;
    private int newAmountOfKnives;
    private Player player;

    [SerializeField] private GameObject thowingKnife; //attackPotion, shieldPotion, redArrowObliquely, redArrowStraight, redArrowLeft, redArrowRight;
    [SerializeField] private GameObject enemySpawners;
    [SerializeField] private GameObject buffSpawners;
    [SerializeField] private GameObject skipTutorialObject, waveMonitor;
    private GameManager gameManager;
    private int isDone
    {
        get => PlayerPrefs.GetInt("DaggerTutorialIsDone", 0);
        set => PlayerPrefs.SetInt("DaggerTutorialIsDone", 1);
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        if (player.PlayerType == Player.TypeOfPlayer.Swordsman)
        {
            gameObject.SetActive(false);
            return;
        }
        if (isDone == 1)
        {
            TutorialIsDone();
            return;
        }

        currentsection = 1;
        animator = GetComponent<Animator>();
        animator.enabled = false;
        Time.timeScale = 0;
        gameManager = FindObjectOfType<GameManager>();
        GameManager.gameIsPaused = true;
        gameManager.pauseDeactivated = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale < 1) ;
        
        if (Input.GetMouseButtonDown(1) && currentsection == 1)
        {
            animator.Play("DaggerTutorial2");
            currentsection++;
            thowingKnife.SetActive(true);
        }
        else if (thowingKnife.gameObject == null && currentsection == 2)
        {
            EndOfTutorial();
        }
        
       
    }
    void TutorialIsDone()
    {
        enemySpawners.SetActive(true);
        buffSpawners.SetActive(true);
        waveMonitor.SetActive(true);
        isDone = 1;
        gameObject.SetActive(false);
    }

    void EndOfTutorial()
    {
        enemySpawners.SetActive(true);
        buffSpawners.SetActive(true);
        waveMonitor.SetActive(true);
        isDone = 1;
        GameManager.gameIsPaused = false;
        gameManager.pauseDeactivated = false;
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void SkipTutorial()
    {
        enemySpawners.SetActive(true);
        buffSpawners.SetActive(true);
        waveMonitor.SetActive(true);
        isDone = 1;
        GameManager.gameIsPaused = false;
        gameManager.pauseDeactivated = false;
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void DontSkipTutorial()
    {
        skipTutorialObject.SetActive(false);
        animator.enabled = true;
        GameManager.gameIsPaused = false;
        gameManager.pauseDeactivated = false;
        Time.timeScale = 1;
    }
}
