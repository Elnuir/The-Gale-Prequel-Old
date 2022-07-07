using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaintWaterCounter : MonoBehaviour
{
    public int amount;
    private string x = "x";
    private Text textWindow;

    private Throw throw1;
    // Start is called before the first frame update
    void Start()
    {
        throw1 = FindObjectOfType<Throw>();
        textWindow = GetComponent<Text>();
        amount = throw1.amountSaintWater;
        textWindow.text = x + amount;
    }

    // Update is called once per frame
    void Update()
    {
        //if(amount==0) return;
    }
    public void TextUpdate()
    {
        if (!textWindow)
            Start();
        amount = throw1.amountSaintWater;

        textWindow.text = x + amount;//Yeah like why not. Used in collectable script
    }

}
