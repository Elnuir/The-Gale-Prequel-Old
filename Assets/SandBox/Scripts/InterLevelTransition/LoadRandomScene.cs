using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadRandomScene : MonoBehaviour
{
    public int[] LoadableSceneBuildIndexes = new int[0];
    private int SelectedBuildIndex = -1;
    public ApplyScreenshot NextScreenshot;
    public LoadScene NextSceneLoader;


    // Start is called before the first frame update
    void Start()
    {
        if (SelectedBuildIndex < 0)
            SelectedBuildIndex = LoadableSceneBuildIndexes[Random.Range(0, LoadableSceneBuildIndexes.Length)];
    }

    public void PrepareLoad()
    {
        if (SelectedBuildIndex < 0)
            Start();

        NextScreenshot.DoShit(SelectedBuildIndex);
        NextSceneLoader.buildIndexOffset= SelectedBuildIndex - SceneManager.GetActiveScene().buildIndex;
    }

    public void WriteSceneNameTo(Text text)
    {
        if (SelectedBuildIndex < 0)
            Start();

        string path = SceneUtility.GetScenePathByBuildIndex(SelectedBuildIndex);
        string sceneName = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
        text.text = sceneName;
    }
}
