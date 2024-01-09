using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public string projectileName;
    public GameObject projectilePrefab;
    public float speed;
    public int damage;
}