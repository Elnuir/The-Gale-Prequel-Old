using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetEffectsVolume : MonoBehaviour
{
    public AudioSettings Audio;

    private Slider _slider;

    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.value = Audio.EffectsVolume;
        _slider.onValueChanged.AddListener(UpdateValue);
    }

    private void UpdateValue(float arg0)
    {
        Audio.EffectsVolume = arg0;
    }
}
