using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTexture : MonoBehaviour
{
    public Camera Cumera;
    public Material Material;
    private RenderTexture _texture;


    private void Start()
    {
        Cumera.CopyFrom(Camera.main);
        _texture = new RenderTexture(Cumera.pixelWidth, Cumera.pixelHeight, 24);
    }

    public void DoShit()
    {
        Cumera.targetTexture = _texture;
        Cumera.Render();
        Material.mainTexture = _texture;
    }

}
