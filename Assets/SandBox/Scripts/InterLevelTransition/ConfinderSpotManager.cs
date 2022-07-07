using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConfinderSpotManager : MonoBehaviour
{
   // public Camera Camera;
    public GameObject Confinder;
    public Transform[] Spots;
    private int _currentIndex = 0;

    private void Start()
    {
        GoToSpot(0);

    }
    public void GoToSpot(int index)
    {
        var cameraSpot = Spots[index].transform.position;
        //cameraSpot.z = Camera.transform.position.z;
        //Camera.transform.position = cameraSpot;

        Confinder.transform.position = Spots[index].transform.position;
        _currentIndex = index;
    }

    public void GoNextSpot()
    {
        _currentIndex = (_currentIndex + 1) % Spots.Length;
        GoToSpot(_currentIndex);
    }
}
