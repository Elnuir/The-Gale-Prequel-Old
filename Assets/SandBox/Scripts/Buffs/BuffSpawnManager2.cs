using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuffSpawnManager2 : MonoBehaviour
{
    public int MinSpawnInterval;
    public int MaxSpawnInterval;

    public Transform[] SpawnSpots;
    public List<GameObject> BuffPrefabs = new List<GameObject>();

    public int SaintWaterBuffStock;
    public int HealthBuffStock;

    private Dictionary<Transform, GameObject> occupied
        = new Dictionary<Transform, GameObject>();

    private float nextSpawnTimer;
    
    void Start()
    {
        SpawnSpots = GameObject.FindGameObjectsWithTag("BuffSpawner").Select(g => g.GetComponent<Transform>()).ToArray();
    }

    private void Update()
    {
        if (nextSpawnTimer <= 0)
        {
            Spawn();
            nextSpawnTimer = Random.Range(MinSpawnInterval, MaxSpawnInterval);
        }
        else
            nextSpawnTimer -= Time.deltaTime;
    }

    void ClearCollectedBuffs()
    {
        foreach (var spot in occupied.Keys.ToArray())
            if (occupied[spot].gameObject == null || !occupied[spot].gameObject.activeSelf)
                occupied.Remove(spot);
        
//        Debug.Log(occupied.Count);
    }

    [CanBeNull]
    Transform ChooseSpotToSpawn()
    {
        ClearCollectedBuffs();
        var freeSpots = SpawnSpots.Where(s => !occupied.ContainsKey(s)).ToArray();

        if (freeSpots.Length > 0)
            return freeSpots[Random.Range(0, freeSpots.Length)];

        return null;
    }

    [CanBeNull]
    GameObject ChooseBuffToSpawn()
    {
       RemoveBuffsOutOfStock();

       if (BuffPrefabs.Count > 0)
           return BuffPrefabs[Random.Range(0, BuffPrefabs.Count)];

       return null;
    }

    void RemoveBuffsOutOfStock()
    {
        if (HealthBuffStock == 0)
            BuffPrefabs.RemoveAll(b => b.CompareTag("HealthCollectable"));
        
        if(SaintWaterBuffStock == 0)
            BuffPrefabs.RemoveAll(b => b.CompareTag("SaintWaterCollectable"));

    }

    void RemoveFromStock(GameObject buff)
    {
        if (buff.CompareTag("HealthCollectable"))
            --HealthBuffStock;

        if (buff.CompareTag("SaintWaterCollectable"))
            --SaintWaterBuffStock;
    }

    void RegisterOccupiedSpot(GameObject buff, Transform spot)
    {
        if(buff.scene.name == null)
            Debug.LogError("You must use instanciated (not a prefab) object here");
        occupied[spot] = buff;
    }

    void Spawn()
    {
        GameObject buffToSpawn = ChooseBuffToSpawn();
        Transform spotToSpawn = ChooseSpotToSpawn();

        if (buffToSpawn != null && spotToSpawn != null)
        {
            var spawnedBuff = buffToSpawn.GetCloneFromPool(null, spotToSpawn.position, Quaternion.identity);
            RemoveFromStock(buffToSpawn);
            RegisterOccupiedSpot(spawnedBuff, spotToSpawn);
        }
    }
    
}