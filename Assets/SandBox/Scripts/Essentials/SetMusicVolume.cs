using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SetMusicVolume : MonoBehaviour
{
    public AudioSettings Audio;

    private Slider _slider;

    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.value = Audio.MusicVolume;
        _slider.onValueChanged.AddListener(UpdateValue);
    }

    private void UpdateValue(float arg0)
    {
        Audio.MusicVolume = arg0;
    }
}
