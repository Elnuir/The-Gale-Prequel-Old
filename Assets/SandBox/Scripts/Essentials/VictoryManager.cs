using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VictoryManager : MonoBehaviour
{
    public UnityEvent ForInteractableScene;
    public UnityEvent ForOrdinaryScene;

    public void DoVirtoryShit()
    {
        if (FindObjectOfType<GameManager>().isInteractableScene)
            ForInteractableScene?.Invoke();
        else
            ForOrdinaryScene?.Invoke();
    }
}
