using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressAnyKeyWin : MonoBehaviour
{
    private GameObject winCanvas;

    public UnityEvent keyPressed;
    //private SettingsManager settingsManager;
    void Start()
    {
        winCanvas = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            Time.timeScale = 1;
            winCanvas.SetActive(false);
            keyPressed?.Invoke();
        }
    }
}
