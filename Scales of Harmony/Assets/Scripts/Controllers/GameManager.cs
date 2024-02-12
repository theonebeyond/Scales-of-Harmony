using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using static GameManager;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set; }
    private float SpawnTimer = 0f;

    // Syndazy
    public string StopName = "StopAll";

    public enum GameState
    {
        MainMenu,
        InGame,
        Paused,
        TraitSelection,
        AttributeSelection,
        EnterBossPhase,
        BossFight,
        SavePhase,
        GameOver
    }
    // Parameters that are used to determine the friendly dragon series:
    public DragonData[] AllDragonList;
    public List<DragonData> selectedDragons = new List<DragonData>();
    public DragonData currentDragon;
    private int currentDragonIndex = 0;
    public GameState currentState;
    public CameraController Camera;



    //Managers to be tracked;
    public DragonController dragonController;
    public EnemySpawnManager enemySpawnManager; // Reference to the EnemySpawnManager
    public UIController uiManager;
    public PowerManager powerManager;
    public PlayerStatManager playerStatManager;
    public DialogueManager dialogueManager;
    public int currentlevel = 0;


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

    private void Awake()
    {
        //Syndazy
        AkSoundEngine.RegisterGameObj(gameObject);
    }
    void Start()
    {
        Time.timeScale = 1;
        GameStartUISetUp();
        FalseToFakeStop(false);
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
                enemySpawnManager.SpawnEnemy(currentlevel);
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
                FalseToFakeStop(true);
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
                uiManager.SwitchSettingButton(false);
                playerStatManager.Initialize_Attribute_Board();

                break;

            // [BOSS Phase] Set Up:
            case GameState.EnterBossPhase:
                ClearAllMobs();
                enemySpawnManager.SpawnBoss(currentlevel);
                BOSSAI boss = FindAnyObjectByType<BOSSAI>();
                boss.enabled = false;
                uiManager.SwitchInGameUI(false);
                uiManager.SwitchMenu(false);
                FalseToFakeStop(false);
                Camera.EnableMoveToFixedPosition();
                break;

            case GameState.BossFight:
                Time.timeScale = 1f;
                GameObject bossboss = GameObject.FindGameObjectWithTag("BOSS");
                BOSSAI bossAI = bossboss.GetComponent<BOSSAI>();
                bossAI.enabled = true;
                InGameUISetting();
                FalseToFakeStop(true);
                uiManager.BossHealthbarSwitch(true);
                Camera.EnableFollow();
                uiManager.ScoreSwitch(false);
                break;
            // [Game Over] Set Up:
            case GameState.GameOver:
                // Handle game over logics
                uiManager.BossHealthbarSwitch(false);
                Debug.Log("GameOver started");
                uiManager.backgroundBase.SetActive(true);
                uiManager.SwitchMenu(false);
                uiManager.DisplayGameOverScreen(true);
                uiManager.SwitchInGameUI(false);
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
        SceneManager.LoadScene("MainMenu");
    }


    public void ResumeGame()
    {
        BOSSAI boss = FindAnyObjectByType<BOSSAI>();
        if (boss == null) { ChangeState(GameState.InGame); }
        else { ChangeState(GameState.BossFight); }

    }

    public void EnterBossPhase()
    {
        ChangeState(GameState.EnterBossPhase);
    }

    public void EnterBossFight()
    {
        ChangeState(GameState.BossFight);
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
        //Syndazy
        AkSoundEngine.PostEvent(StopName, gameObject);
       //AkSoundEngine.UnregisterGameObj(gameObject);

        ChangeState(GameState.GameOver);
    }

    public void ShowButton(Boolean OnOff)
    {
        uiManager.SwitchSettingButton(OnOff);

    }
    public void GameStartUISetUp()
    {
        uiManager.WinGameSwitch(false);
        uiManager.backgroundBase.SetActive(false);
        uiManager.DisplayGameOverScreen(false);
        uiManager.buddyInJail.SetActive(false);
        uiManager.BossHealthbarSwitch(false);
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
    private void InGameUISetting()
    {
        uiManager.BossHealthbarSwitch(false);
        uiManager.SwitchInGameUI(true);
        uiManager.SwitchMenu(false);
        uiManager.AttributeMenuSwitch(false);
        uiManager.ScoreSwitch(true);
        uiManager.SwitchTraitSelectionUI(false, currentDragon);
        ShowButton(true);
    }

    public void DialogueUISetting()
    {
        uiManager.backgroundBase.SetActive(false);
        uiManager.DisplayGameOverScreen(false);
        uiManager.BossHealthbarSwitch(false);
        uiManager.SwitchDialogueBox(false);
        uiManager.SwitchInGameUI(false);
        uiManager.SwitchMenu(false);
        uiManager.SwitchTraitSelectionUI(false, currentDragon);
        uiManager.AttributeMenuSwitch(false);
        uiManager.DialogueBoxSwitch(true);
    }

    void SelectRandomDragons()
    {
        List<DragonData> dragonsPool = new List<DragonData>(AllDragonList);
        for (int i = 0; i <= 2; i++)
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

    void ClearAllMobs()
    {
        GameObject[] mobs = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject mob in mobs)
        {
            Destroy(mob);
        }
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
    }

    public void FalseToFakeStop(bool OnOff)
    {
        dragonController.StopMovement();
        BlessingControl[] blessings = FindObjectsOfType<BlessingControl>(true); // Set 'true' to include inactive objects
        foreach (BlessingControl blessing in blessings)
        {
            blessing.gameObject.SetActive(OnOff);
        }

        dragonController.enabled = OnOff;
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

    public void DialogueButtonEnterBossFight()
    {
        // Start the game with the first dragon
        uiManager.DialogueBoxSwitch(false);
        uiManager.BuddyJailSwitch(false);
        ChangeState(GameState.BossFight);
    }
    public void DialogueButtonAfterTalkingToBuddy()
    {
        // Start the game with the first dragon
        uiManager.BuddyObjectSwitch(false);
        uiManager.DialogueBoxSwitch(false);
        EncounterDragon();
        Camera.EnableFollow();
        Camera.ResetBool();
    }
    public void BossDied() 
    {
        ClearAllMobs();
        currentlevel++;
        FalseToFakeStop(false);
        playerStatManager.score = 0;
        uiManager.BossHealthbarSwitch(false);
        uiManager.SwitchInGameUI(false);
        Camera.EnableMoveToDragon();
        
    }

    public void GameWin() 
    {
        Time.timeScale = 0;
        uiManager.WinGameSwitch(true);
        uiManager.SwitchInGameUI(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

