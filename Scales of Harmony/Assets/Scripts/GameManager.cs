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
        AttributeSelection,
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
    public PlayerStatManager playerStatManager;
    public DialogueManager dialogueManager;
    public int playerLevel = 1;


    /*
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
    */
    void Start()
    {
        Time.timeScale = 1;
        GameStartUISetUp();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Game paused by shortcut");
            PauseGame();
        }
        
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

    //
    private void HandleStateChange(GameState newState)
    {
        switch (newState)
        {
            // [Main Menu] Set Up:
            case GameState.MainMenu:
                //uiManager.SwitchTraitSelectionUI(false, currentDragon);
                uiManager.DisplayGameOverScreen(false);
                Time.timeScale = 0;
                uiManager.SwitchMenu(true);
                uiManager.SwitchInGameUI(false);
                Debug.Log("Engered menu");
                // Handle main menu logic
                break;

            // [In Game] Set Up:
            case GameState.InGame:
                Time.timeScale = 1;
                InGameUISetting();

                break;

            // [Paused] Set Up:
            case GameState.Paused:
                //PauseGame();
                break;

            // [Trait Selection] Set Up:
            case GameState.TraitSelection:
                ShowButton(false);
                Time.timeScale = 0f;
                uiManager.SwitchTraitSelectionUI(true, currentDragon);
                Debug.Log("Trait select done");
                break;


            // [Attribute Selection] Set Up:            
            case GameState.AttributeSelection:
                Time.timeScale = 0f;
                playerStatManager.Initialize_Attribute_Board();

                break;

            // [BOSS Phase] Set Up:
            case GameState.BossPhase:
                break;

            // [Game Over] Set Up:
            case GameState.GameOver:
                // Handle game over logic
                Debug.Log("GameOver started");
                uiManager.backgroundBase.SetActive(true);
                uiManager.SwitchMenu(false);
                uiManager.DisplayGameOverScreen(true);
                uiManager.SwitchInGameUI(false);
                break;
        }
    }

    public void GameStartUISetUp() 
    {
        uiManager.backgroundBase.SetActive(false);
        uiManager.DisplayGameOverScreen(false);
        uiManager.SwitchDialogueBox(false);
        uiManager.SwitchInGameUI(false);
        uiManager.SwitchMenu(false);
        uiManager.SwitchTraitSelectionUI(false, currentDragon);
        uiManager.AttributeMenuSwitch(false);
        uiManager.WelcomeTXTSwitch(true);
        uiManager.printWelcome();
        uiManager.InGameAttributePointUpdate(playerStatManager.AttributePoint);
        Debug.Log("Game Initiated");
    }
    public void PauseGame()
    {
        ChangeState(GameState.MainMenu);
        Time.timeScale = 0;
    }


    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void ResumeGame()
    {
        ChangeState(GameState.InGame);
    }

    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("MainScene");
    }

    public void AttributeAdd()
    {
        ChangeState(GameState.AttributeSelection);
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
        
    }

    public void ShowButton(Boolean OnOff)
    {
        uiManager.SwitchSettingButton(OnOff);

    }

    private void InGameUISetting()
    {
        uiManager.SwitchInGameUI(true);
        uiManager.SwitchMenu(false);
        uiManager.AttributeMenuSwitch(false);
        uiManager.SwitchTraitSelectionUI(false, currentDragon);
        ShowButton(true);
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

    public void WelcomeEnd() {
        uiManager.WelcomeTXTSwitch(false);
        Debug.Log("About to display welcome");
        uiManager.DialogueBoxSwitch(true);
        dialogueManager.DisPlayWelcomeDialogue();


    }

    public void WelcomeDialogueEnd()
    {
        // Start the game with the first dragon
        uiManager.DialogueBoxSwitch(false);
        SelectRandomDragons();
        EncounterDragon();
    }
}

