using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public string projectileName;
    public GameObject projectilePrefab;
    public Boolean TruePlayerFalseEnemy;
    public float speed;
    public int damage;
    public float range; // Add this field to set the travel distance
}