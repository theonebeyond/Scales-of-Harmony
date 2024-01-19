using UnityEngine;

[CreateAssetMenu(fileName = "New Boss", menuName = "Boss")]
public class BossData : ScriptableObject
{
    public string bossName;
    public int health;
    public int attackDamage;
    public float fireRate;
    public bool isProjectile;
    public float movingSpeed;
    public bool IsFinal;
    public GameObject bossPrefab;
    public ProjectileData projectile;// If you want to have a visual representation in the editor
    // You can add more fields as needed, such as special moves, attack patterns, etc.
}
