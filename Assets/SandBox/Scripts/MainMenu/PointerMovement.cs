using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PointerMovement : MonoBehaviour
{
    //private MainMenuManager menuManager;
    // public int currentPosition = 0;
    //  public int maxAmountOfPositions;
    // private Vector3 _currVelocity;

    public Transform Dragger;

    void OnEnable()
    {
        // var a = GameObject.Find("MainMenuManager");
        // menuManager = a.GetComponent<MainMenuManager>();
        Invoke(nameof(MoveToFirstAvailableLevel), 0.03f);
    }


    void MoveToFirstAvailableLevel()
    {
        var obj = FindObjectsOfType<SetIsProcessed>()
        .Where(s =>
        {
            var component = s.GetComponent<PassedLevelScheck>();
            return component.isAvailable && !component.isPassed;
        })
        .OrderBy(s => s.Level).FirstOrDefault();

        if (obj)
        {
            transform.position = new Vector2(obj.transform.position.x, obj.transform.position.y + 90f);
            Dragger.transform.position = new Vector2(Dragger.transform.position.x, transform.position.y);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.RightArrow))
        // {
        //     currentPosition++;
        // }
        // else if (Input.GetKeyDown(KeyCode.LeftArrow))
        // {
        //     currentPosition--;
        // }
        // currentPosition = Mathf.Clamp(currentPosition, 0, menuManager.pointerPositions.Length - 1);

        //transform.position = Vector3.SmoothDamp(transform.position, new Vector2(menuManager.pointerPositions[currentPosition].transform.position.x, menuManager.pointerPositions[currentPosition].transform.position.y + 200), ref _currVelocity,0.2f );
    }
}
