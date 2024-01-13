using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Projectile : MonoBehaviour
{
    public ProjectileData projectileData;
    public PlayerStatManager playerStatManager;
    private Vector2 direction;
    private float travelDistance;
    private Vector2 startPosition;

    public void Initialize(Vector2 target, ProjectileData data)
    {
        playerStatManager = FindAnyObjectByType<PlayerStatManager>();
        startPosition = transform.position;
        projectileData = data;
        direction = (target - (Vector2)transform.position).normalized; // Direction towards the target
        travelDistance = projectileData.range; // Getting range from ProjectileData
        if (projectileData.TruePlayerFalseEnemy)
        {
            travelDistance += playerStatManager.Range[0];
            // Adjusting the size based on playerStatManager.Range[0]
            float sizeMultiplier = 0.05f * playerStatManager.Range[0] + 1;
            transform.localScale *= sizeMultiplier; // Multiplying the current scale by the size multiplier
        }

    }

    private void Start()
    {
        playerStatManager = FindAnyObjectByType<PlayerStatManager>();
    }
    void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        if (Vector2.Distance(startPosition, transform.position) < travelDistance)
        {
            transform.position += (Vector3)direction * projectileData.speed * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject); // Destroy the projectile after reaching the travel distance
        }
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!(projectileData.TruePlayerFalseEnemy) && (collider.gameObject.CompareTag("Dragon"))) 
        {
            PlayerStatManager player = FindAnyObjectByType<PlayerStatManager>();
            if (player != null)
            {
                player.TakeDamage(projectileData.damage);
            }

            Destroy(gameObject);

        }
        if (projectileData.TruePlayerFalseEnemy && collider.gameObject.CompareTag("Enemy"))
        {
            // The enemy has a script to handle taking damage
            EnemyAI enemyAI = collider.gameObject.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(projectileData.damage);
            }

            Destroy(gameObject);
        } 

    }
}
        