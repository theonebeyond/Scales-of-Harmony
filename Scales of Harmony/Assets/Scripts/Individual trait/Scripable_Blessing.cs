using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Blessing", menuName = "Blessing")]
public class Blessing : ScriptableObject
{
    public string blessingName;
    public Sprite blessingSprite; // Sprite of the blessing object
    public string description;
    public int blessingNo;
    public int DragonNo;
    public float attackInterval;
    public ProjectileData projectileData;
    public float effectValue; // Damage for attackers, block value for defenders, etc.
    public int pierceCount;

    // Additional parameters based on the blessing type
}