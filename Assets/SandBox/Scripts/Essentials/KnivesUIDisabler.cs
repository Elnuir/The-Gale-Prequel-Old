using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnivesUIDisabler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(FindObjectOfType<Player>().PlayerType == Player.TypeOfPlayer.Swordsman)
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
