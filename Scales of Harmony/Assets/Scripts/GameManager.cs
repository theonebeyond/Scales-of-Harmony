using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using static GameManager;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set; }
    private float SpawnTimer = 0f;
    public enum GameState
    {
        MainMenu,
        InGame,
        Paused,
        TraitSelection,
        BossPhase,
        SavePhase,
        GameOver
    }
    // Parameters that are used to determine the friendly dragon series:
    public DragonData[] AllDragonList;
    private List<DragonData> selectedDragons = new List<DragonData>();
    private DragonData currentDragon;
    private int currentDragonIndex = 0;

    public GameState currentState;

    //Managers to be tracked;
    public EnemySpawnManager enemySpawnManager; // Reference to the EnemySpawnManager
    public UIController uiManager;
    public PowerManager powerManager;
    public int playerLevel = 1;



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
        SelectRandomDragons();
        // Start the game with the first dragon

        ChangeState(GameState.InGame);
        EncounterDragon();
    }

    void Update()
    {
        if (currentState == GameState.InGame)
        {
            SpawnTimer += Time.deltaTime;
            if (SpawnTimer >= enemySpawnManager.spawnInterval)
            {
                enemySpawnManager.SpawnEnemy();
                SpawnTimer = 0f;
            }

        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        HandleStateChange(newState);
    }

    private void HandleStateChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                Time.timeScale = 0;
                uiManager.SwitchMenu(true);
                uiManager.SwitchInGameUI(false);
                // Handle main menu logic
                break;
            case GameState.InGame:
                Time.timeScale = 1;
                uiManager.SwitchInGameUI(true);
                uiManager.SwitchMenu(false);
                uiManager.SwitchTraitSelectionUI(false, currentDragon);
                /*
                uiManager.SwitchMainDisplay(true);
                uiManager.SwitchOfTraitDisplay(true);
                PauseMainSceneGameplay(true);
                ResumeGame();
                */
                break;
            case GameState.Paused:
                //PauseGame();
                break;
            case GameState.TraitSelection:
                Time.timeScale = 0;
                uiManager.SwitchTraitSelectionUI(true, currentDragon);
                /*
                uiManager.SwitchOfTraitDisplay(false);
                traitManager.OfferTraitChoices();
                PauseGame();
                */
                break;
            case GameState.BossPhase:
                // Hand over control to BattleManager
                /*
                FoodSpawner foodSpawner = FindAnyObjectByType<FoodSpawner>();
                foodSpawner.CancelInvoke();
                uiManager.SwitchMainDisplay(false);
                PauseMainSceneGameplay(false);
                DestroyAllFood();
                Debug.Log("everything done");
                SceneManager.LoadScene("BattleScene");

                // Use the sceneLoaded event to find the BattleManager when the "BattleScene" has loaded
                SceneManager.sceneLoaded += OnBattleSceneLoaded;
                Debug.Log("+= stuff finished");
                */
                break;
            case GameState.GameOver:
                // Handle game over logic
                //DisplayGameOverScreen();
                break;
        }
    }

    public void PauseGame()
    {
        ChangeState(GameState.MainMenu);
        Time.timeScale = 0;
    }

    public void ExitGame()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("MainMenu");
    }


    public void ResumeGame()
    {
        ChangeState(GameState.InGame);
    }

    public void RestartGame()
    {
        Destroy(gameObject);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("MainScene");
    }

    void SelectRandomDragons()
    {
        List<DragonData> dragonsPool = new List<DragonData>(AllDragonList);
        for (int i = 0; i < 4; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, dragonsPool.Count);
            selectedDragons.Add(dragonsPool[randomIndex]);
            Debug.Log(dragonsPool[randomIndex].name);
            dragonsPool.RemoveAt(randomIndex);

        }
    }

    void EncounterDragon()
    {
        if (currentDragonIndex < selectedDragons.Count)
        {
            currentDragon = selectedDragons[currentDragonIndex];
            // Handle the encounter with currentDragon
            // and assigning the first blessing from currentDragon.blessings
            ChangeState(GameState.TraitSelection);

            currentDragonIndex++;
        }
    }

    public DragonData GetCurrentDragon()
    {
        return currentDragon;

    }
}

