using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public void CompleteLevel(int level)
    {
        PlayerPrefs.SetInt($"level-{level}", 1);

    }
}
