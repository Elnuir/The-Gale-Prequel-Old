using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackFrontManager : MonoBehaviour
{
    public string[] BackgroundObjects = { };

    public string[] FrontgroundObjects = { };

    private GameObject[] _bgObjects;

    private GameObject[] _fgObjects;

    private void Start()
    {
        _bgObjects = BackgroundObjects.Select(GameObject.Find).ToArray();
        _fgObjects = FrontgroundObjects.Select(GameObject.Find).ToArray();

        if (_bgObjects.Any(b => b == null))
            Debug.LogError("Блять проверь какого-то background объекта нету");

        if (_fgObjects.Any(b => b == null))
            Debug.LogError("Блять проверь какого-то foreground объекта нету");
    }

    public void EnableAll()
    {
        SetForegroundActive(true);
        SetBackgroundActive(true);
    }

    public void DisableAll()
    {
        SetForegroundActive(false);
        SetBackgroundActive(false);
    }

    public void ShowFrontground()
    {
        SetForegroundActive(true);
        SetBackgroundActive(false);
    }

    public void ShowBackground()
    {

        SetForegroundActive(false);
        SetBackgroundActive(true);
    }
    
    private void SetForegroundActive(bool active) {

        foreach (var o in _fgObjects)
            o?.SetActive(active);
    }

    private void SetBackgroundActive(bool active) {
        
        foreach (var o in _bgObjects)
            o?.SetActive(active);
    }
}
