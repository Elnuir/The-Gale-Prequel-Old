using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplyScreenshot : MonoBehaviour
{
    public Material ApplyBgTo;
    public Material ApplyFgTo;

    public string FolderPath;


    public void DoShit(int nextLvlBuildIndex)
    {
        string filePathFg = GetActualFilePath(FolderPath, nextLvlBuildIndex, "-fg");
        string filePathBg = GetActualFilePath(FolderPath, nextLvlBuildIndex, "-bg");
        var resultTextureFg = LoadPNG(filePathFg);
        var resultTextureBg = LoadPNG(filePathBg);
        ApplyFgTo.mainTexture = resultTextureFg;
        ApplyBgTo.mainTexture = resultTextureBg;
    }

    public string GetActualFilePath(string folderPath, int buildIndex, string postfix)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(buildIndex);
        string sceneName = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);

        // TODO: figure out why this shity doesn't work
        // var scenes = SceneManager.GetAllScenes();

        // int buildIndex = SceneManager.GetActiveScene().buildIndex + 1;
        // var scene = SceneManager.GetSceneAt(buildIndex);
        string fileName = $"{sceneName}{postfix}.jpg"; //"newTest2.jpg";// TODO: fix thisshit !!!!!!!!!!!!!!!!!!!!!!  !!!!!!!!!!! $"{scene.name}.jpg";
        string actualPath = Path.Combine(folderPath, fileName);

        if (File.Exists(actualPath))
            return actualPath;
        else
        {
            Debug.LogError($"ПИЗДЕЦ файла {actualPath} не существует");
            return null;
        }
        // return path;
    }

    public Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }
}
