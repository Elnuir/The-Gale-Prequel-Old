using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonCheckboxEmulator : MonoBehaviour
{
    public Button[] Buttons;
    public bool AutoButtons;

    private void Start()
    {
        if (AutoButtons)
            Buttons = GetComponentsInChildren<Button>();
        SubscribeAll();
    }

    private void SubscribeAll()
    {
        foreach (var b in Buttons)
            b.onClick.AddListener(() => ClickedHandler(b));
    }

    private void ClickedHandler(Button source)
    {
        foreach (var b in Buttons)
            if (b != source)
                b.interactable = true;
        source.interactable = false;
    }
}
