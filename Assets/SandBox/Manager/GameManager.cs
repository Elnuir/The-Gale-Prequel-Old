using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    // [SerializeField] private float respawnTime;
    public static bool gameIsPaused;
    public bool pauseDeactivated;
    [SerializeField] private GameObject pauseCanvas;

    public UnityEvent OnWin;
    private float respawnTimeStart;
    private bool respawn;

    private Player player;

    public GameObject waveCounterCanvas;

    private Transform winCanvas;
    private Transform deathCanvas;

    [SerializeField] private EnemySpawnManager enemySpawnManager;
    // [SerializeField] private BuffSpawnManager buffSpawnManager;

    [SerializeField] private GameObject[] playersToSpawn;

    [SerializeField] public bool isInteractableScene;
    // public int playerIndex;

    private void Awake()
    {

    }

    void Start()
    {

        player = FindObjectOfType<Player>();
        // attack = FindObjectOfType<PlayerAttack>();
        // startBuffTime = attackBufftimeLeft;
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        winCanvas = Resources.FindObjectsOfTypeAll<Transform>().FirstOrDefault(g =>
            g.gameObject.scene.buildIndex == sceneIndex &&
            g.gameObject.name == "WinCanvas");  //Finds disabled winCanvas
        deathCanvas = Resources.FindObjectsOfTypeAll<Transform>().FirstOrDefault(
            g =>
                g.gameObject.scene.buildIndex == sceneIndex &&
                g.gameObject.name == "DeathCanvas");  //Finds disabled winCanvas

    }

    public void EnemyKilledHandler()
    {
        enemySpawnManager.activeEnemies--;
    }

    private void Update()
    {
        //        print(gameIsPaused);
        if (player.isDead)
        {
            Invoke(nameof(RunDeathCanvas), 4f);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseDeactivated)
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }


    void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
            pauseCanvas.SetActive(true);
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1;
            pauseCanvas.SetActive(false);
            AudioListener.pause = false;
        }
    }

    public void ContinueButton()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
        AudioListener.pause = false;
    }

    public void ExitButton()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
        AudioListener.pause = false;
        MapCanvasEnabler.isEnabledMapCanvas = false;
        SceneManager.LoadScene("MainMenu");
    }


    void RunDeathCanvas()
    {
        if (!winCanvas.gameObject.activeSelf)
            deathCanvas.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RunWinCanvas()
    {
        OnWin?.Invoke();
    }

    public void SpawningCharacterOnStart(int playerIndex)
    {
        GameObject spawnedPlayer = Instantiate(playersToSpawn[playerIndex], spawnPoint.position, quaternion.identity);
        if (isInteractableScene)
        {
            spawnedPlayer.GetComponent<Rigidbody2D>().simulated = false;
            //  spawnedPlayer.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
            //spawnedPlayer.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            spawnedPlayer.GetComponent<PlayerCombatManager>().canBeUsed = false;
            spawnedPlayer.GetComponentInChildren<Throw>().canBeUsedKnives = false;
            spawnedPlayer.GetComponentInChildren<Throw>().canBeUsedWater = false;
        }
    }
}

