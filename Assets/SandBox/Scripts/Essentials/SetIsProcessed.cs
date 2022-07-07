using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PassedLevelScheck))]
public class SetIsProcessed : MonoBehaviour
{
    private PassedLevelScheck _passed;
    public int[] LevelsToOpen = new int[0];
    public int[] LevelsToClose = new int[0];

    public int Level;

    // Start is called before the first frame update
    void Start()
    {
        _passed = GetComponent<PassedLevelScheck>();
        Set();
        OpenLevels();

        Invoke(nameof(CloseLevels), 0f); //Time.deltaTime+float.Epsilon);
    }

    void Set()
    {
        if (!PlayerPrefs.HasKey($"level-{Level}"))
            Debug.Log($"Oh fuck, level {Level} doesn't exist ");

        _passed.isPassed = PlayerPrefs.GetInt($"level-{Level}") == 1;

        if (PlayerPrefs.GetInt($"level-{Level}") > 1)
            Debug.Log("You fucked up, check the script which completes level (and check ur ass too)");
    }
    void CloseLevels()
    {
        if (!_passed.isPassed) return;

        var targets = FindObjectsOfType<SetIsProcessed>();

        foreach (var t in targets)
        {
            if (LevelsToClose.Contains(t.Level))
                t.GetComponent<PassedLevelScheck>().isAvailable = false;
            
        }
    }
    void OpenLevels()
    {
        if (!_passed.isPassed) return;

        var targets = FindObjectsOfType<SetIsProcessed>();

        foreach (var t in targets)
        {
            if (LevelsToOpen.Contains(t.Level))
                t.GetComponent<PassedLevelScheck>().isAvailable = true;
            
        }
    }
    
}