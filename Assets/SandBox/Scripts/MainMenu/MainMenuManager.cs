using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    //private GameObject MapCanvas;
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject areYouSure;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject mapCanvas;
    [SerializeField] private GameObject cameraDragger;
    // public GameObject[] pointerPositions; //What was from start don't u fucking touch it


    void Start()
    {
        Time.timeScale = 1;
        if (MapCanvasEnabler.isEnabledMapCanvas)
        {
            menuCanvas.SetActive(false);
            mapCanvas.SetActive(true);
            cameraDragger.SetActive(true);
        }
        else if (!MapCanvasEnabler.isEnabledMapCanvas)
        {
            menuCanvas.SetActive(true);
            mapCanvas.SetActive(false);
            cameraDragger.SetActive(false);
        }
    }

    public void Reset()
    {
        // float a = FindObjectOfType<AudioSettings>().MusicVolume;   //
        // float b = FindObjectOfType<AudioSettings>().EffectsVolume; // CRUTCH

        // PlayerPrefs.DeleteAll();

        // FindObjectOfType<AudioSettings>().MusicVolume = a; // CRUTCH 2 TODO: NORMAL
        // FindObjectOfType<AudioSettings>().EffectsVolume = b; //

        PlayerPrefsUtils.DeleteAllBut(new[] {
            (PlayerPrefsUtils.ItemType.Float, AudioSettings.MusicVolumeKey),
            (PlayerPrefsUtils.ItemType.Float, AudioSettings.EffectsVolumeKey)
            });

        MapCanvasEnabler.isEnabledMapCanvas = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //menuCanvas.SetActive(false);
        //cameraDragger.SetActive(true);
        //mapCanvas.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cameraDragger.SetActive(false);
            menuCanvas.SetActive(true);
            mapCanvas.SetActive(false);
        }
    }

    public void GoToHub()
    {
        SceneManager.LoadScene("Hub");
    }

    public void NewGame()
    {
        if (PlayerPrefs.HasKey("full-score"))
        {
            main.SetActive(false);
            areYouSure.SetActive(true);
        }
        else
        {
            Reset();
        }
    }

    public void ResetAbsolutelyEverythingBlyat()
    {
        PlayerPrefs.DeleteAll();
    }
}
