using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using TMPro;
public class EnemyAI : MonoBehaviour
{
    public EnemyData enemyData;
    public GameObject soulOrbPrefab;
    public GameObject player;
    public PlayerStatManager playerStatManager;
    public GameManager gameManager;
    public GameObject CritText;
    public SpriteRenderer spriteRenderer;
    private float nextFireTime;
    private int maxHealth;
    private int currentHealth;
    private UIController uIController;
    public Slider healthBar;
    public float spawnRadius = 1.0f;
    private Vector2 lastPosition;
    public int currentlevel;
    void Start()
    {
        uIController = FindAnyObjectByType<UIController>();
        playerStatManager = FindAnyObjectByType<PlayerStatManager>();
        gameManager = FindAnyObjectByType<GameManager>();
        currentlevel = gameManager.currentlevel+1;
        player = GameObject.FindGameObjectWithTag("Dragon");
        nextFireTime = Time.time + enemyData.fireRate;
        maxHealth = enemyData.health*currentlevel;
        currentHealth = enemyData.health; // initialize the enemy health
        UpdateHealthBar();
        lastPosition = transform.position;

    }

    void Update()
    {

        player = GameObject.FindGameObjectWithTag("Dragon"); // Re-acquire the player if lost
        if (player == null) return; // Skip update if player is still not found

        MoveTowardsPlayer();

        if (enemyData.isProjectile && Time.time > nextFireTime)
        {
            FireProjectile();
            nextFireTime = Time.time + enemyData.fireRate;
        }

        // Call FaceDirection
        FaceDirection();

        // Update the last position for the next frame
        lastPosition = transform.position;
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyData.movingSpeed * Time.deltaTime);

    }

    void FireProjectile()
    {
        GameObject projectileObject = Instantiate(enemyData.projectile.projectilePrefab, transform.position, Quaternion.identity);
        projectileObject.GetComponent<Projectile>().Initialize(player.transform.position, enemyData.projectile); // use the scriptable object data
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
            //SpawnFloatingText(this.transform.position,"Crit! " + critDamage + " damage");
            //Debug.Log("Critical strike: raw damage" + modified + " and critdamage: " + critDamage);
        }
        //Debug.Log("enemy took modified damage " + modified + ", and raw damage is " + raw);
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Play die animation
        // Drop orbs
        // Spawn soul orbs
        for (int i = 0; i < enemyData.dropOrbAmount* currentlevel; i++)
        {
            Vector2 spawnPosition = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle * spawnRadius;
            Instantiate(soulOrbPrefab, spawnPosition, Quaternion.identity);
        }
        // Increment score
        playerStatManager.ScoreUp();
        Destroy(gameObject);
    }

    public void SpawnFloatingText(Vector3 position, string text)
    {
        GameObject textObj = Instantiate(CritText, position, Quaternion.identity);
        textObj.GetComponent<TextMeshProUGUI>().text = text; // For UI Text
        // Or use the following line for TextMeshPro
        // textObj.GetComponent<TMPro.TextMeshProUGUI>().text = text;
    }
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
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
}