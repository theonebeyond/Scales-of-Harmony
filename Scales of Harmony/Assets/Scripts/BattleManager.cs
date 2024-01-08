using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public BattleSnake player; // Reference to the BattleSnake script
    public Enemy enemy; // Reference to the Enemy script
    BattleStat battleStat;
    private enum BattleState
    {
        Start,
        PlayerTurn,
        EnemyTurn,
        Won,
        Lost
    }

    private BattleState state;

    void Start()
    {
        player = FindAnyObjectByType<BattleSnake>();
        enemy = FindAnyObjectByType<Enemy>();
        battleStat = FindAnyObjectByType<BattleStat>();
    }

    public IEnumerator BeginBattle()
    {
        yield return new WaitForSeconds(2f);
        state = BattleState.Start;
        SetupBattle();
        Debug.Log("setupbattle called");
    }
    void SetupBattle()
    {
        // Set up the initial state of the battle, like positioning the player and enemy
        player = FindAnyObjectByType<BattleSnake>();
        enemy = FindAnyObjectByType<Enemy>();
        battleStat = FindAnyObjectByType<BattleStat>();
        Debug.Log("about to setupbattle in enemy");
        enemy.SetupBattle();
        state = BattleState.PlayerTurn;
        Debug.Log("Reached SetupBattle");
        StartCoroutine(PlayerTurn());
    }

    IEnumerator PlayerTurn()
    {

        if (player == null)
        {
            Debug.LogError("Player reference not set in BattleManager.");
            yield break; // Exit the coroutine
        }

        yield return StartCoroutine(player.ExecuteTurn(this));

        // Check if enemy is defeated
        if (enemy.health <= 0)
        {
            state = BattleState.Won;
            EnemyDefeated();
        }
        else
        {
            state = BattleState.EnemyTurn;

            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {

        if (enemy == null)
        {
            Debug.LogError("Enemy reference not set in BattleManager.");
            yield break; // Exit the

        }
            yield return StartCoroutine(enemy.ExecuteTurn());

        // Check if player is defeated
        if (player.health <= 0)
        {
            state = BattleState.Lost;
            PlayerDefeated();
        }
        else
        {
            state = BattleState.PlayerTurn;
            Debug.Log("Enemy had performed an attack");
            StartCoroutine(PlayerTurn());
        }
    }

    void EndBattle()
    {
        if (state == BattleState.Won)
        {
            Debug.Log("Player won!");
            // Handle victory
        }
        else if (state == BattleState.Lost)
        {
            Debug.Log("Player lost!");
            // Handle defeat
        }
    }

    public void PlayerLeveledUp()
    {
        // Handle player leveling up if applicable
    }

    public void EnemyDefeated()
    {
        enemy.health = 0;
        battleStat.UpdateGameLevel();
        state = BattleState.Won;
        EndBattle();
    }

    public void PlayerDefeated()
    {
        player.health = 0;
        state = BattleState.Lost;
        EndBattle();
    }
}