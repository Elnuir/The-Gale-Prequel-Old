using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class PassedLevelScheck : MonoBehaviour
{
    private Button button;
    public bool isPassed;
    public bool isAvailable;
    [SerializeField] private GameObject cross;
    //[SerializeField] private string[] typeOfLevel;
    [SerializeField] private int levelId;
    

    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        if (!isAvailable)
        {
            button.interactable = false;
        }
        else
        {
            if (isPassed)
            {
               button.interactable = false;
                cross.SetActive(true);
            }
            else
            {
                button.interactable = true;
                cross.SetActive(false);
            }
        }
    }

    public void LoadScene()
    {
        MapCanvasEnabler.isEnabledMapCanvas = true;
        var levelNumber = GetComponent<SetIsProcessed>().Level;

        CurrentLevel.CurrLevel = levelNumber;
        
        if (levelId == 0)
        {
            SceneManager.LoadScene("Invaders");
        }
        else if (levelId == 1)
        {
            var a = Random.Range(1, 3);
            switch (a)
            {
                case 1:
                    SceneManager.LoadScene("EliteInvaders1");
                    break;
                case 2:
                    SceneManager.LoadScene("EliteInvaders2");
                    break;
            }
        }
        else if (levelId == 2)
        {
            SceneManager.LoadScene("Rest");
        }
        else if (levelId == 3)
        {
            SceneManager.LoadScene("Obscurity");
            // var a = Random.Range(0, SceneManager.sceneCountInBuildSettings);
            // while (a == 0 || a == 1 || a == 3 || a == 7 || a == 8 || a== 9)
            // {
            //     a = Random.Range(0, SceneManager.sceneCountInBuildSettings);
            // }
            // SceneManager.LoadScene(a);
        }
        else if (levelId == 4)
        {
            SceneManager.LoadScene("Dealer");
        }
        else if (levelId == 5)
        {
            SceneManager.LoadScene("Treasure");
        }
        else if (levelId == 6)
        {
            SceneManager.LoadScene("Castle");
        }
        else if (levelId == 7)
        {
            SceneManager.LoadScene("Tutorial");
        }
        else if (levelId == 8)
        {
            SceneManager.LoadScene("Cave");
        }
        else if (levelId == 9)
        {
            SceneManager.LoadScene("SwampForest");
        }
        // else if (levelId == 10)
        // {
        //     SceneManager.LoadScene("EliteInvaders2");
        // }
        else
        {
            SceneManager.LoadScene("Boss");
        }
        
    }

    public void LoadFirstScene()
    {
        SceneManager.LoadScene("Invaders");
    }

    public void LoadSceneByIntex(int index)
    {
        SceneManager.LoadScene(index);
    }

}
