using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraSpotManager : MonoBehaviour
{
    public Transform CameraTarget;
    public GameObject Cofinder;
    public Transform[] Spots;
    private int _currentIndex = 0;

    private void Start()
    {
        if (Spots.Contains(CameraTarget))
            Debug.LogWarning("Чел твой cumerа таргет не должен быть в массиве спотоа");

        GoToSpot(0);

    }
    public void GoToSpot(int index)
    {
        CameraTarget.transform.position = Spots[index].transform.position;
        if (Cofinder != null)
            Cofinder.transform.position = Spots[index].transform.position;
        _currentIndex = index;
    }

    public void GoNextSpot()
    {
        _currentIndex = (_currentIndex + 1) % Spots.Length;
        CameraTarget.transform.position = Spots[_currentIndex].transform.position;
        if (Cofinder != null)
            Cofinder.transform.position = Spots[_currentIndex].transform.position;
    }

}
