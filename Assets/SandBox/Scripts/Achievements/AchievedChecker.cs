using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievedChecker : MonoBehaviour
{
    [SerializeField] private Text demonKillerText, ghostHunterText, exorcismText,youShallNotPassText, richGuyText, bersekText;
    private AchievementsManager achievementsManager;
    void Start()
    {
        achievementsManager = FindObjectOfType<AchievementsManager>();
        Check();
    }

    public void Check()
    {
        if (achievementsManager.DemonKillerIsShown == 1)
        {
            demonKillerText.color = new Color(255, 255, 255);
        }
        if (achievementsManager.GhostHunterIsShown == 1)
        {
            ghostHunterText.color = new Color(255, 255, 255);
        }
        if (achievementsManager.ExorcismIsShown == 1)
        {
            exorcismText.color = new Color(255, 255, 255);
        }
        if (achievementsManager.YouShallNotPassIsShown == 1)
        {
            youShallNotPassText.color = new Color(255, 255, 255);
        }
        if (achievementsManager.RichGuyIsShown == 1)
        {
            richGuyText.color = new Color(255, 255, 255);
        }
        if (achievementsManager.BersekIsShown == 1)
        {
            bersekText.color = new Color(255, 255, 255);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
