using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffSpawnManager : MonoBehaviour
{
    private Transform[] buffSpawners;
    [SerializeField] private float dropTimeBase;
    private float dropTimeLeft;
    [SerializeField] private GameObject[] buffs;

    private bool isStopped;
    [SerializeField] private int[] buffsOnWaves;

    [SerializeField] private EnemySpawnManager enemySpawnManager;
    private List<Transform> chosenBuffSpawners = new List<Transform>();
    public int activeBuffs;
    BuffContainer[] buffContainers;
    void Start()
    {
        buffSpawners = GameObject.FindGameObjectsWithTag("BuffSpawner").Select(g => g.GetComponent<Transform>())
            .ToArray();
        dropTimeLeft = dropTimeBase;
        buffContainers = FindObjectsOfType<BuffContainer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStopped)
        {
            SpawnCheck();
        }
        TrackOfCollectedBuffs();
    }

    void SpawnCheck()
    {
        dropTimeLeft -= Time.deltaTime;
        if (dropTimeLeft <= 0)
        {
            dropTimeLeft = dropTimeBase;
            SpawnReady();
        }
    }

    void SpawnReady()
    {
        var piece = buffSpawners.OrderBy(a => Random.Range(0, 1000)).Take(buffsOnWaves[enemySpawnManager.currentWave])
            .ToArray();
        chosenBuffSpawners.AddRange(piece);

        foreach (var buffSpawner in chosenBuffSpawners)
        {
            GameObject e = buffs[Random.Range(0, buffs.Length)];
            e.GetCloneFromPool(null, buffSpawner.position, Quaternion.identity);
            activeBuffs++;
        }
        isStopped = true; //DROPTIME MUST BE MORE THEN 3
        dropTimeLeft = dropTimeBase;
        chosenBuffSpawners.Clear();
    }
    private void TrackOfCollectedBuffs()
    {
        if (isStopped && activeBuffs == 0)
        {
            isStopped = false;
        }
    }
}