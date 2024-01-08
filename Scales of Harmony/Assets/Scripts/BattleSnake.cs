using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleSnake : MonoBehaviour
{
    public GameObject snakeTailPrefab;
    private BattleStat battleStat;
    private BattleManager battleManager;
    private List<GameObject> bodySegments = new List<GameObject>();
    private GameObject snakeTail;
    public float segmentSpacing = 1.0f;
    public float health = 100;
    private List<Vector3> bodyPositions = new List<Vector3>();
    private List<TraitData> activeTraits = new List<TraitData>();
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        Debug.Log("Let me let me let me start the snakesnake");
        battleStat = FindObjectOfType<BattleStat>(); // Assuming there's only one BattleStat instance
        health = battleStat.health;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "snakehead";
        AssembleSnake();
    }

    void AssembleSnake()
    {
        List<TraitData> currentTraits = battleStat.GetCurrentTraits();
        foreach (TraitData currentTrait in currentTraits)
        {
            if (currentTrait.TraitType == 1 )
            {
                activeTraits.Add(currentTrait);
            }

        }
        Vector3 nextPosition = transform.position;

        for (int i = 0; i < activeTraits.Count; i++)
        {
            if (activeTraits[i].TraitType == 1)
            {
                Vector3 direction = Vector3.zero;
                if (i == 0)
                {
                    // First segment goes downwards
                    direction = Vector3.down;
                }
                else if (i <= 3)
                {
                    // Next three segments go to the left
                    direction = Vector3.left;
                }
                else if (i <= 5)
                {
                    // Next two segments go downwards
                    direction = Vector3.down;
                }
                else
                {
                    // Remaining segments go to the right
                    direction = Vector3.right;
                }

                nextPosition += direction * segmentSpacing; // Update position for next segment
                GameObject segment = Instantiate(activeTraits[i].segmentPrefab, nextPosition, Quaternion.identity);
                SpriteRenderer BodySpriteRenderer = segment.GetComponent<SpriteRenderer>();
                BodySpriteRenderer.sortingLayerName = "snakebody"; 
                bodyPositions.Add(nextPosition);
                bodySegments.Add(segment);
            }
        }

        // Add the tail at the end
        nextPosition += Vector3.down * (segmentSpacing * 4/5); // Position tail to the right of the last segment
        snakeTail = Instantiate(snakeTailPrefab, nextPosition, Quaternion.Euler(0f, 0f, 180f));
        SpriteRenderer TailSpriteRenderer = snakeTail.GetComponent<SpriteRenderer>();
        TailSpriteRenderer.sortingLayerName = "snaketail";
    }
    public IEnumerator ExecuteTurn(BattleManager battleManager)
    {
        //PlayerAttack();
        foreach (var segment in bodySegments)
        {
            PerformAttack(segment, battleManager.enemy);
            yield return new WaitForSeconds(0.5f); // Wait time between attacks
        }

        yield return null; // End of the snake's turn
    }

    void PerformAttack(GameObject segment, Enemy enemy)
    {
        // Logic for the segment's attack
        // For example, dealing damage based on the trait the segment represents
        
        Debug.Log("Player had performed an attack");
        int damage = CalculateDamage(segment);
        enemy.TakeDamage(damage);
    }

    int CalculateDamage(GameObject segment)
    {
        // Placeholder logic for damage calculation
        // Modify this to use trait-specific data
        return 10; // Example damage value
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public List<Vector3> getBodyPositions() {
        // Get body positions
        return this.bodyPositions;
    }
    private void Die()
    {
        // Logic for the snake's defeat
        BattleManager battleManager = FindAnyObjectByType<BattleManager>();
        battleManager.PlayerDefeated();
        Debug.Log("Player has been defeated!");
        // Notify BattleManager or handle game over logic
    }
    // Additional methods to update the snake during gameplay if needed
}