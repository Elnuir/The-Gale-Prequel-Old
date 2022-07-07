using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCanvasFix : MonoBehaviour
{
    public bool isFacingRight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation.eulerAngles.y > 100 && !isFacingRight)
        {
            isFacingRight = true;
            transform.Rotate(0, 180, 0);
        }
           
        if (transform.rotation.eulerAngles.y < 100 && isFacingRight)
        {
            isFacingRight = false;
            transform.Rotate(0, 180, 0);
        }
    }
}
