using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaUprisingProxy : MonoBehaviour
{
    private LavaUprising uprising;

    // Start is called before the first frame update
    void Start()
    {
        uprising = FindObjectOfType<LavaUprising>();
        if (!uprising)
            Debug.LogError("I don't give a shit where lava uprising is");
    }

    public void Rebound(float amount)
    {
        if (!uprising)
            uprising = FindObjectOfType<LavaUprising>();

        uprising.Rebound(amount);
    }

}
