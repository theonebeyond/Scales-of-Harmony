using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BOSSAI : MonoBehaviour
{
    public BossData bossData;
    public GameObject soulOrbPrefab;
    public GameObject player;
    public PlayerStatManager playerStatManager;
    public GameManager gameManager;
    public SpriteRenderer spriteRenderer;
    public GameObject CritText;
    private float nextFireTime;
    private int maxHealth;
    private int currentHealth;
    private UIController uIController;
    public float spawnRadius = 1.0f;
    private Vector2 lastPosition;
    private int currentLevel;
    void Start()
    {
        uIController = FindAnyObjectByType<UIController>();
        playerStatManager = FindAnyObjectByType<PlayerStatManager>();
        player = GameObject.FindGameObjectWithTag("Dragon");
        gameManager = FindAnyObjectByType<GameManager>();
        nextFireTime = Time.time + bossData.fireRate;
        currentLevel = gameManager.currentlevel + 1;    
        maxHealth = bossData.health* currentLevel;
        currentHealth = maxHealth; // initialize the enemy health
        UpdateHealthBar();
        lastPosition = transform.position;
        if (gameManager.currentState == GameManager.GameState.EnterBossPhase) { this.enabled = false; }

    }
    void Update()
    {

        player = GameObject.FindGameObjectWithTag("Dragon"); // Re-acquire the player if lost
        if (player == null) return; // Skip update if player is still not found

        MoveTowardsPlayer();

        if (bossData.isProjectile && Time.time > nextFireTime)
        {
            FireProjectile();
            nextFireTime = Time.time + bossData.fireRate;
        }

        // Call FaceDirection
        FaceDirection();

        // Update the last position for the next frame
        lastPosition = transform.position;
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, bossData.movingSpeed * Time.deltaTime);

    }

    void FireProjectile()
    {
        GameObject projectileObject = Instantiate(bossData.projectile.projectilePrefab, transform.position, Quaternion.identity);
        projectileObject.GetComponent<Projectile>().Initialize(player.transform.position, bossData.projectile); // use the scriptable object data
    }

    public void TakeDamage(int damage)
    {
        int raw = damage;
        float modified = raw * (1 + ((float)playerStatManager.AttackPower[0] / 10));
        System.Random random = new System.Random();
        int randomValue = random.Next(1, 101);
        if (randomValue > playerStatManager.CritChance[0])
        {
            currentHealth -= (int)modified;
        }
        if (randomValue <= playerStatManager.CritChance[0])
        {
            int critDamage = (int)(modified * 2);
            currentHealth -= critDamage;
            //SpawnFloatingText(this.transform.position, "Crit! " + critDamage + " damage");
            Debug.Log("Critical strike: raw damage" + modified + " and critdamage: " + critDamage);
        }
        if (currentHealth <= 0)
        {
            Die();
        }
        Debug.Log("enemy took modified damage " + modified + ", and raw damage is " + raw);
        UpdateHealthBar();

    }

    void Die()
    {
        // Play die animation
        // Drop orbs
        // Spawn soul orbs
        Destroy(gameObject);
        GameManager gameManager = FindAnyObjectByType<GameManager>();
        if ( bossData.IsFinal) { gameManager.GameWin(); }
        currentLevel++;
        for (int i = 0; i < 50*currentLevel; i++)
        {
            Vector2 spawnPosition = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle * spawnRadius;
            Instantiate(soulOrbPrefab, spawnPosition, Quaternion.identity);
        }
        // Increment score
        gameManager.BossDied();


    }

    private void UpdateHealthBar()
    {
        if (uIController != null)
        {
            uIController.UpdateBossHealth(currentHealth , maxHealth);
        }
    }


    private void FaceDirection()
    {
        Vector2 moveDirection = (Vector2)transform.position - lastPosition;
        if (moveDirection.x > 0)
        {
            // Moving right
            spriteRenderer.flipX = false;
        }
        else if (moveDirection.x < 0)
        {
            // Moving left
            spriteRenderer.flipX = true;
        }
    }

    public void SpawnFloatingText(Vector3 position, string text)
    {
        GameObject textObj = Instantiate(CritText, position, Quaternion.identity);
        textObj.GetComponent<TextMeshProUGUI>().text = text; // For UI Text
        // Or use the following line for TextMeshPro
        // textObj.GetComponent<TMPro.TextMeshProUGUI>().text = text;
    }
    void DisplayIntroDialogue()
    {
        // Code to display boss's intro dialogue

    }

    void ExecuteAttackPattern()
    {
        // Implement the boss's attack patterns
        // This could include normal attacks, special moves, etc.
    }



}