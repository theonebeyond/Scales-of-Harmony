using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Projectile : MonoBehaviour
{
    public ProjectileData projectileData;

    private Vector2 targetPosition;

    public void Initialize(Vector2 target, ProjectileData data)
    {
        targetPosition = target;
        projectileData = data;
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        if (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            transform.position += (Vector3)direction * projectileData.speed * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerStatManager>().TakeDamage(projectileData.damage);
        }
        Destroy(gameObject);
    }
}
