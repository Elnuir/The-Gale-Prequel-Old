using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject spawnedEntity;
    public GameObject[] entity;
    [SerializeField] private int spawnProbability;

    [SerializeField] float dropTimeLeftMin = 10.0f;
    [SerializeField] float dropTimeLeftMax = 20.0f;
    [SerializeField] private float dropTimeLeft;
    private float dropStartTime;
    [SerializeField] bool isPausedSpawnTimer;

    private bool isFirstSpawnDone;
    private float firstSpawnTime;
    [SerializeField] float fistSpawnTimeMin = 10f;
    [SerializeField] float fistSpawnTimeMax = 20f;
    private GameManager gameManager;
    public GameObject portal;
    
    static List<GameObject> _activeEnemies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        firstSpawnTime = Random.Range(fistSpawnTimeMin, fistSpawnTimeMax);
        dropTimeLeft = Random.Range(dropTimeLeftMin, dropTimeLeftMax);
        dropStartTime = dropTimeLeft;
    }

    // ~Spawner()
    // {
    //     _activeEnemies.Clear();
    //     Debug.Log("Cleanup static stuff");
    // }

    private void OnDestroy()
    {
        _activeEnemies.Clear();
        Debug.Log("ACTIVE  ENEMIES HAS BEEN CLEANED"); //CAN CAUSE BUGS ATTENTION!!!
    }

    // Update is called once per frame
    void Update()
    {
        Pause();
        Continue();
        FirstSpawn();
        BufforEnemySpawn();
    }

    public void SpawnCheck()
    {
        if (!entity[0].CompareTag("Enemy"))
            isPausedSpawnTimer = false;

        if (isPausedSpawnTimer) return;

        
        var a = Random.Range(0, 100);


        if (spawnedEntity != null && spawnedEntity.gameObject && spawnedEntity.activeSelf) return;

        if (a < spawnProbability)
        {
            if (entity[1].gameObject.CompareTag("Enemy"))
            {
                portal.GetCloneFromPool(transform, new Vector2(transform.position.x + 0.5f, transform.position.y), Quaternion.identity);
            }

            Invoke(nameof(Spawn), 2f);
        }

//        print(a);
    }

    public void Spawn()
    {
        if (isPausedSpawnTimer) return;

        spawnedEntity = entity[Random.Range(0, entity.Length)].GetCloneFromPool(transform, transform.position, Quaternion.identity);

        // if (spawnedEntity.tag == "Enemy")
        // Debug.Log("entity is drawn");

        spawnedEntity.transform.parent = null;

        if (spawnedEntity.CompareTag("Enemy") && !_activeEnemies.Contains(spawnedEntity))
        {
            _activeEnemies.Add(spawnedEntity);
        }
    }
    

    void BufforEnemySpawn()
    {
        if (!isPausedSpawnTimer && isFirstSpawnDone)
        {
            dropTimeLeft -= Time.deltaTime;
            if (dropTimeLeft < 0)
            {
                SpawnCheck();
                dropTimeLeft = Random.Range(dropTimeLeftMin, dropTimeLeftMax);
                dropTimeLeft = dropStartTime;
            }
        }
    }

    void FirstSpawn()
    {
        if (!isFirstSpawnDone)
        {
            firstSpawnTime -= Time.deltaTime;
            if (firstSpawnTime <= 0)
            {
                isFirstSpawnDone = true;
                SpawnCheck();
            }
        }
    }


    // void ClearThrash()
    // {
    //     _activeEnemies.RemoveAll(e => !e.activeSelf);
    //
    //     if (spawnedEntity != null && spawnedEntity.gameObject?.activeSelf != true)
    //         spawnedEntity = null;
    // }

    public void Pause()
    {
        //ClearThrash();
      //  int enemyCount = _activeEnemies.Count;

//        Debug.Log(gameManager.killsPerWave[gameManager.CurrentWave] + "   " + enemyCount);
        // if (gameManager.killsPerWave[gameManager.CurrentWave] <= enemyCount)
        // {
        //     if (entity[0].CompareTag("Enemy"))
        //         isPausedSpawnTimer = true;
        // }
        
       // Debug.Log($"{enemyCount} of {gameManager.killsPerWave[gameManager.CurrentWave] }");

    }

    void Continue()
    {
       // ClearThrash();
       // int enemyCount = _activeEnemies.Count;

        // if (gameManager.killsPerWave[gameManager.CurrentWave] > enemyCount)
        // {
        //     if (entity[0].CompareTag("Enemy"))
        //     isPausedSpawnTimer = false;
        // }
        

    }
}