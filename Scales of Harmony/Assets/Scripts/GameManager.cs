using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        MainMenu,
        InGame,
        Paused,
        TraitSelection,
        BattlePhase,
        GameOver
    }

    public GameState currentState;
    public TraitManager traitManager;
    public BattleManager battleManager; // Reference to the BattleManager
    public UIManager uiManager;
    public int playerLevel = 1;
    public float gameTimer = 60f; // 1 minute for the food eating game

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
        ChangeState(GameState.InGame);
    }

    void Update()
    {
        if (currentState == GameState.InGame)
        {
            gameTimer -= Time.deltaTime;
            uiManager.UpdateTimer(gameTimer);
            if (gameTimer <= 0f)
            {
                ChangeState(GameState.BattlePhase);
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
                // Handle main menu logic
                break;
            case GameState.InGame:
                uiManager.SwitchMainDisplay(true);
                uiManager.SwitchOfTraitDisplay(true);
                PauseMainSceneGameplay(true);
                ResumeGame();
                break;
            case GameState.Paused:
                PauseGame();
                break;
            case GameState.TraitSelection:
                uiManager.SwitchOfTraitDisplay(false);
                traitManager.OfferTraitChoices();
                PauseGame();
                break;
            case GameState.BattlePhase:
                // Hand over control to BattleManager
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
                break;
            case GameState.GameOver:
                // Handle game over logic
                DisplayGameOverScreen();
                break;
        }
    }

    private void OnBattleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BattleScene")
        {
            // Unsubscribe from the event to avoid multiple calls
            SceneManager.sceneLoaded -= OnBattleSceneLoaded;

            // Find the BattleManager now that the scene has loaded
            battleManager = FindAnyObjectByType<BattleManager>();

            if (battleManager != null)
            {
                StartCoroutine(battleManager.BeginBattle());
                Debug.Log("Found and initialized BattleManager.");
            }
            else
            {
                Debug.LogError("BattleManager not found in the 'BattleScene'.");
            }
        }
    }
    public void DestroyAllFood()
    {
        GameObject[] foodItems = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject food in foodItems)
        {
            Destroy(food);
        }
    }
    public void PlayerLeveledUp()
    {
        ChangeState(GameState.TraitSelection);
    }

    private void PauseGame()
    {
        // Logic to pause the game (e.g., stop snake movement)
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        // Logic to resume the game (e.g., resume snake movement)
        Time.timeScale = 1;
        // Reset the timer for the next round
    }

    public void StartBattlePhase()
    {
        SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
        StartCoroutine(SetupBattle());
    }


    IEnumerator SetupBattle()
    {
        yield return new WaitForSeconds(1); // Wait for the scene to load

        // Initialize the battle here
        // Find and set up player and enemy
        // You can use GameObject.FindGameObjectWithTag or similar methods
    }

    public void EndBattlePhase()
    {
        SceneManager.UnloadSceneAsync("BattleScene");
        // Any cleanup or state reset post-battle
    }

    private void PauseMainSceneGameplay(Boolean onOFF)
    {
        // Disable or pause main scene gameplay elements

        // Example: Disable the snake controller script
        var snakeController = FindObjectOfType<SnakeController>();
        if (snakeController != null)
        {
            snakeController.enabled = onOFF;
        }

        var FoodController = FindObjectOfType<FoodSpawner>();
        if (FoodController != null)
        {
            FoodController.enabled = onOFF;
        }

        var snake = GameObject.FindGameObjectWithTag("snake"); // Replace "Snake" with your snake's tag
        if (snake != null)
        {
            snake.SetActive(onOFF);
        }

        var SnakeHead = GameObject.FindGameObjectWithTag("SnakeHead"); // Replace "Snake" with your snake's tag
        if (SnakeHead != null)
        {
            SnakeHead.SetActive(onOFF);
        }

        var food = GameObject.FindGameObjectWithTag("Food"); // Replace "Snake" with your snake's tag
        if (food != null)
        {
            food.SetActive(onOFF);
        }
        // Disable other gameplay elements, like enemy movement, timers, etc.
        // You might also want to pause any animations or ongoing effects

    }
    private void DisplayGameOverScreen()
    {
        // Logic to display the game over screen with summary
    }

    // Other methods as needed...
}
