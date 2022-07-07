using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawnManagerForSections : MonoBehaviour
{

    [Serializable]
    public class EntitySpawnInfo
    {
        public string PrefabName;
        public float SpawnProbability;
    }

    [Serializable]
    public class EntityInfo
    {
        public Vector2 PortalOffset;
        public GameObject Prefab;
    }


    [Serializable]
    public class Wave
    {
        public EntitySpawnInfo[] EntitySpawnInfos;
        public UnityEvent WaveCompleted;
    }


    [Serializable]
    public class Section
    {
        public Transform[] EnemySpawnPoints;
        public Wave[] Waves;

        public override string ToString()
        {
            return $"Waves: {Waves?.Length} Spawn points: {EnemySpawnPoints?.Length}";
        }
    }

    private Queue<(GameObject, Vector2)> _bodySpawnQueue = new Queue<(GameObject, Vector2)>();

    public Section[] Sections;
    public EntityInfo[] SpawnableEntities;
    public GameObject PortalPrefab;

    private int _currentWave;

    private bool _isPaused;


    public void Pause()
    {
        _isPaused = true;
    }

    public void Resume()
    {
        _isPaused = false;
        SpawnEntityFull("Nibbler");
    }

    public void Start()
    {
    }

    private void SpawnEntityFull(string prefabName)
    {
        var entity = GetEntityInfoByName(prefabName);
        Vector2 spawnBodyPoint = GetSpawnBodyPoint();

        Vector2 portalSpawnPoint = GetSpawnPortalPoint(spawnBodyPoint, entity);
        SpawnEntityPortal(portalSpawnPoint);

        _bodySpawnQueue.Enqueue((entity.Prefab, spawnBodyPoint));
        Invoke(nameof(SpawnEntityBody), 1.5f);
    }

    private void SpawnEntityPortal(Vector3 spawnPoint)
    {
        var portal = PortalPrefab.GetCloneFromPool(null, spawnPoint, Quaternion.identity);
    }

    private void SpawnEntityBody()
    {
        var (entity, spawnTransform) = _bodySpawnQueue.Dequeue();
        entity.GetCloneFromPool(null, spawnTransform, Quaternion.identity);
    }

    private Vector2 GetSpawnPortalPoint(Vector2 spawnBodyPoint, EntityInfo entityInfo)
    {
        return spawnBodyPoint + (Vector2)entityInfo.PortalOffset;
    }

    private Vector2 GetSpawnBodyPoint()
    {
        var currentSection = GetCurrentSection();
        var spawnPoints = currentSection.EnemySpawnPoints;
        Transform t = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        return t.position;
    }

    private Section GetCurrentSection()
    {
        int waveCounter = 0;
        int sectionCounter = 0;

        foreach (var section in Sections)
        {
            waveCounter += section.Waves.Length;
            sectionCounter++;

            if (waveCounter > _currentWave)
                return section;
        }

        return null;
    }

    private EntityInfo GetEntityInfoByName(string name)
    {
        EntityInfo result = SpawnableEntities.FirstOrDefault(e => e.Prefab.name == name);

        if (result == null)
            Debug.LogError($"Can't find spawnable prefab with name {name}");

        return result;
    }

}
