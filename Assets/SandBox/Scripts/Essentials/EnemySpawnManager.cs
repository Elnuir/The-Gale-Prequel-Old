using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

[Serializable]
public class WaveEvent
{
    public int WaveNumber = 1;
    public UnityEvent @event;
}

public class EnemySpawnManager : MonoBehaviour
{
    //    [SerializeField] private EntityToSpawn[] entitiesNew;
    private Transform[] enemySpawners;
    [SerializeField] private float dropTimeBase;
    private float dropTimeLeft;
    [SerializeField] private GameObject portal;

    [FormerlySerializedAs("entity")] [SerializeField]
    private GameObject[] entities;

    [SerializeField] [Range(0f, 1f)] private float[] probabilities;

    public WaveEvent[] WaveEvents = new WaveEvent[0];
    public UnityEvent NextWave;

    public int activeEnemies;

    public int currentWave = 1;
    [SerializeField] private int[] enemiesOnWaves;

    private bool isStopped;
    private List<Transform> chosenEnemySpawners = new List<Transform>();
    [SerializeField] private GameObject waveMonitor;
    [SerializeField] private WaveCounter waveCounter;
    [SerializeField] private GameManager gameManager;

    private bool WinCanvasHasBeenShown = false;

    void Start()
    {
        enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner").Select(g => g.GetComponent<Transform>())
            .ToArray();
        dropTimeLeft = dropTimeBase;
        OnCurrentWaveChanged();
    }

    void Update()
    {
        if (!isStopped)
        {
            SpawnCheck();
        }

        TrackOfKilledEnemies();
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
        // var piece = enemySpawners.Where(s => Random.Range(0, 2) == 1).ToArray();

        //int count            = currentWave < enemiesOnWaves.Length ? enemiesOnWaves[currentWave] : 0;
        var piece = enemySpawners.OrderBy(a => Random.Range(0, 1000)).Take(enemiesOnWaves[currentWave]).ToArray();
        chosenEnemySpawners.AddRange(piece);

        foreach (var enemySpawner in chosenEnemySpawners)
        {
            portal.GetCloneFromPool(null, enemySpawner.position, Quaternion.identity);
        }

        Invoke(nameof(Spawn), 2f);
    }

    void Spawn()
    {
        foreach (var enemySpawner in chosenEnemySpawners)
        {
            //var e = entities[Random.Range(0, entities.Length)];
            var e = ChooseEnemyToSpawn();
            Vector2 spawnPosition = default;
            if (e.TryGetComponent(out NRedeerMovement a))
            {
                spawnPosition = new Vector2(enemySpawner.position.x - 0.61f, enemySpawner.position.y + 2.23f);
            }
            else if (e.TryGetComponent(out NNiblerMovement b))
            {
                spawnPosition = new Vector2(enemySpawner.position.x - 0.91f, enemySpawner.position.y + 2.02f);
            }
            else if (e.TryGetComponent(out NPossessedHealth c))
            {
                spawnPosition = new Vector2(enemySpawner.position.x - 0.6f, enemySpawner.position.y + 1.9f);
            }

            e.GetCloneFromPool(null, spawnPosition, Quaternion.identity);

            activeEnemies++;
        }

        isStopped = true; //DROPTIME MUST BE MORE THEN 3
        dropTimeLeft = dropTimeBase;

        chosenEnemySpawners.Clear();
    }

    GameObject ChooseEnemyToSpawn()
    {
        float probSum = 0f;

        float rnd = Random.Range(0f, 1f);
        for (int i = 0; i < entities.Length; ++i)
        {
            float currProb = GetEnemySpawnProbability(entities[i]);
            if (rnd >= probSum && rnd <= probSum + currProb)
            {
                // if (i == 2) ;
                // Debug.Log($"random is {rnd},between {probSum} and {probSum + currProb}, so ima spawn {entities[i].name}");
                return entities[i];
            }
            else
                probSum += currProb;
        }

        Debug.Log("Блять");
        return null;
    }

    float GetEnemySpawnProbability(GameObject e)
    {
        if (e.TryGetComponent<AchievementHandler>(out var handler))
        {
            switch (handler.EnemyType)
            {
                case AchievementHandler.MobType.Nibbler:
                    return probabilities[0];
                case AchievementHandler.MobType.Redeer:
                    return probabilities[1];
                case AchievementHandler.MobType.Possessed:
                    return probabilities[2];
            }
        }

        Debug.LogError(e.name + "has wrong type");
        return 0f;
    }

    private void TrackOfKilledEnemies()
    {
        if (isStopped)
        {
            if (activeEnemies == 0)
            {
                WaveCompleted();
            }
        }
    }

    private void OnCurrentWaveChanged()
    {
        if (WaveEvents == null)
            return;

        var wave = WaveEvents.FirstOrDefault(w => w.WaveNumber == currentWave);

        if (wave != null)
            wave.@event?.Invoke();
    }

    void WaveCompleted()
    {
        currentWave++;
        OnCurrentWaveChanged();
        if (!WinCanvasHasBeenShown && currentWave > enemiesOnWaves.Length - 1)
        {
            Invoke(nameof(RunWinCanvas), 3f);
            WinCanvasHasBeenShown = true;
            return;
        }

        if (!WinCanvasHasBeenShown)
        {
            waveCounter.WaveUpdate();
            waveMonitor.SetActive(true);
        }

        isStopped = false;
        NextWave?.Invoke();
    }

    void RunWinCanvas()
    {
        gameManager.RunWinCanvas();
    }

    //enemySpawners[Random.Range(0, enemySpawners.Length)]
}