using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public int buildIndexOffset = 1;

    public void DoShit()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + buildIndexOffset);
    }
}
