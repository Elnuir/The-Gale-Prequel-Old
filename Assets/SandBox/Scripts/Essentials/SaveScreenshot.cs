using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveScreenshot : MonoBehaviour
{
    public Camera Cumera;
    private RenderTexture _texture;
    public string folderPath;
    public float Delay;
    private string _currentDoShitPostfix;

    private int _currBuildIndex;

    private void Awake()
    {
        DontDestroyOnLoad(Cumera);
        DontDestroyOnLoad(Cumera.gameObject);
        DontDestroyOnLoad(gameObject);
        _texture = new RenderTexture(Cumera.pixelWidth, Cumera.pixelHeight, 24);
    }

    private void OnlyOneInstance()
    {
        foreach (var o in FindObjectsOfType<SaveScreenshot>())
            if (o._currBuildIndex < _currBuildIndex)
                GameObject.Destroy(o.gameObject);
    }

    private void Start()
    {
        OnlyOneInstance();
        DisablePlayer();
        DisableCanvas();

        Cumera.CopyFrom(Camera.main);

        Invoke(nameof(DoAllKindOfShit), Delay);
    }

    public void GoNextScene()
    {
        int count = SceneManager.sceneCountInBuildSettings;

        if (++_currBuildIndex >= count) return;

        SceneManager.LoadScene(_currBuildIndex);
        Invoke(nameof(Start), Delay);
    }

    private void DisablePlayer()
    {
        var p = FindObjectOfType<Player>();
        if (p != null)
            p.gameObject.SetActive(false);
    }

    private void DisableCanvas()
    {
        var manager = FindObjectOfType<GameManager>();
        foreach (var c in FindObjectsOfType<Canvas>())
        {
            if (manager != null && !manager.isInteractableScene)
                c.gameObject.SetActive(false);
        }
    }

    private void DoAllKindOfShit()
    {
        Invoke(nameof(DoShitWithBackground), Delay);
    }

    private void DoShitWithBackground()
    {
        var manager = FindObjectOfType<BackFrontManager>();

        if (manager)
            manager.ShowBackground();

        _currentDoShitPostfix = "-bg";
        Invoke(nameof(DoShit), Delay);
        Invoke(nameof(DoShitWithFrontground), Delay + 0.5f);
    }

    private void DoShitWithFrontground()
    {
        var manager = FindObjectOfType<BackFrontManager>();

        if (manager) {
            manager.ShowFrontground();
            Invoke(nameof(DisablePlayer), 0.1f);
        }
        _currentDoShitPostfix = "-fg";
        Invoke(nameof(DoShit), Delay);
        Invoke(nameof(GoNextScene), Delay + 0.5f);
    }

    public static Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        var old_rt = RenderTexture.active;
        RenderTexture.active = rTex;

        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();

        RenderTexture.active = old_rt;
        return tex;
    }

    public void DoShit()
    {
        Cumera.targetTexture = _texture;
        Cumera.Render();

        Texture2D image = toTexture2D(_texture);
        byte[] data = image.EncodeToJPG(100);
        File.WriteAllBytes(GetActualFilePath(folderPath, _currentDoShitPostfix), data);

    }

    public static string GetActualFilePath(string folderPath, string postfix)
    {
        var scene = SceneManager.GetActiveScene();
        string fileName = $"{scene.name}{postfix}.jpg";
        string path = Path.Combine(folderPath, fileName);
        return path;
    }
}
