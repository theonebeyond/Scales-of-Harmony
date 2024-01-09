using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int health;
    public bool isProjectile;
    public GameObject enemyPrefab;
    public ProjectileData projectile;
    public float fireRate;
    public float movingSpeed;
    public int collisionDamage;
    public AnimationClip dieAnimation;
    public int dropOrbAmount;
}

