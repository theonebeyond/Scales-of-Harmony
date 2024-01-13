using UnityEngine;

public class BOSSAI : MonoBehaviour
{
    public BossData bossData;
    private int currentHealth;
    // Other necessary private variables (e.g., target player, attack timers)

    void Start()
    {
        currentHealth = bossData.health;
        // Initialize other components (e.g., Animator, Rigidbody)
        DisplayIntroDialogue();
    }

    void Update()
    {
        // Handle AI behavior (e.g., movement, attacking)
        ExecuteAttackPattern();
    }

    void DisplayIntroDialogue()
    {
        // Code to display boss's intro dialogue
        Debug.Log(bossData.introDialogue);
    }

    void ExecuteAttackPattern()
    {
        // Implement the boss's attack patterns
        // This could include normal attacks, special moves, etc.
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(bossData.defeatDialogue);
        // Code for the boss's defeat (e.g., play animation, drop rewards)
        // Call method to handle post-boss fight logic (e.g., saving the dragon, gaining EXP)
    }
}
