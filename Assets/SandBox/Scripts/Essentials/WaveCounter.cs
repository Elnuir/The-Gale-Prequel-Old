using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCounter : MonoBehaviour
{
    private Text text;
    private string prefix = "Wave: ";
    public int currentWave;
    // [SerializeField] private GameObject[] IntersectionalTeleports;

    [SerializeField] private EnemySpawnManager enemySpawnManager;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        text.text = prefix + currentWave;
    }

    public void WaveUpdate()
    {
        currentWave = enemySpawnManager.currentWave;
        text.text = prefix + currentWave;
        // IntersectionalTeleports[currentWave].SetActive(true);
    }
    
}
