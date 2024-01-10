using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Projectile : MonoBehaviour
{
    public ProjectileData projectileData;
    private Vector2 direction;
    private float travelDistance;

    private Vector2 startPosition;

    public void Initialize(Vector2 target, ProjectileData data)
    {

        startPosition = transform.position;
        projectileData = data;
        direction = (target - (Vector2)transform.position).normalized; // Direction towards the target
        travelDistance = projectileData.range; // Assuming you add a 'range' field to ProjectileData

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
        if (projectileData.TruePlayerFalseEnemy && (collider.gameObject.CompareTag("Enemy"))) Destroy(gameObject); 
        if (!(projectileData.TruePlayerFalseEnemy) && (collider.gameObject.CompareTag("Dragon"))) Destroy(gameObject);
    }
}
        