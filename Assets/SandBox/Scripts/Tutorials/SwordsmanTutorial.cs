using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwordsmanTutorial : MonoBehaviour
{
    private int currentsection;
    Animator animator;
    private DashMove dashMove;
    private Player player;
    private int newAmountOfKnives;
    private int amountofjumps = 2;

    [SerializeField] private GameObject healthPotion; //attackPotion, shieldPotion, redArrowObliquely, redArrowStraight, redArrowLeft, redArrowRight;
    [SerializeField] private GameObject enemySpawners;
    [SerializeField] private GameObject buffSpawners;
    [SerializeField] private GameObject skipTutorialObject, waveMonitor;
    private GameManager gameManager;

    private int isDone
    {
        get => PlayerPrefs.GetInt("SwordsmanTutorialIsDone", 0);
        set => PlayerPrefs.SetInt("SwordsmanTutorialIsDone", 1);
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        if (player.PlayerType == Player.TypeOfPlayer.Dagger)
        {
            gameObject.SetActive(false);
            return;
        }
        if (isDone == 1)
        {
            TutorialIsDone();
            return;
        }
        
        amountofjumps = 2;
        currentsection = 1;
        animator = GetComponent<Animator>();
        animator.enabled = false;
        dashMove = FindObjectOfType<DashMove>();
        player = FindObjectOfType<Player>();
        Time.timeScale = 0;
        gameManager = FindObjectOfType<GameManager>();
        GameManager.gameIsPaused = true;
        gameManager.pauseDeactivated = true;
        
    }
    void Update()
    {
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && currentsection == 1 && !skipTutorialObject.activeSelf)
        {
            animator.Play("SwordsmanTutorial2");
            currentsection++;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && currentsection == 2)
        {
            animator.Play("SwordsmanTutorial3");
            currentsection++;
        }
        else if (currentsection == 3)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                amountofjumps--;
            }

            if (amountofjumps <= 0)
            {
                //  print("sas");
                animator.Play("SwordsmanTutorial4");
                currentsection++;
            }
        }
        else if (dashMove.isDashing && currentsection == 4)
        {
            animator.Play("SwordsmanTutorial5");
            currentsection++;
        }
        else if (Input.GetMouseButtonDown(0) && currentsection == 5)
        {
            healthPotion.SetActive(true);
            animator.Play("SwordsmanTutorial6");
            currentsection++;
        }
        else if (healthPotion.gameObject == null && currentsection == 6)
        {
            currentsection++;
            animator.Play("SwordsmanTutorial7");
        }
        else if (
            Input.GetKeyDown(KeyCode.Alpha1) && currentsection == 7)
        {
            currentsection++;
            animator.Play("SwordsmanTutorial8");
            Invoke(nameof(EndOfTutorial), 3f);
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