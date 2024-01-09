using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public EnemyData enemyData;
    public GameObject player;
    private float nextFireTime;
    private int currentHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Dragon");
        nextFireTime = Time.time + enemyData.fireRate;
        currentHealth = enemyData.health; // initialize the enemy health
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
        enemyData.health -= damage;
        if (enemyData.health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Play die animation
        // Drop orbs
        Destroy(gameObject);
    }
}