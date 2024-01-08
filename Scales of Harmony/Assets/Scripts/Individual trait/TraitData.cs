using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New TraitData", menuName = "Trait Data", order = 51)]
public class TraitData : ScriptableObject
{
    public string traitName;
    public Sprite icon;
    public GameObject segmentPrefab; // For active traits
    public int TraitType;
    public TraitQuality initialQuality;
    public int minLevel;
    public string description;


    /* TraitType:
     * 1: Active
     * 2. Stat
     * 3. Passive
     */
    // Enum for trait quality
    public enum TraitQuality
    {
        Normal,
        Great,
        Excellent,
        Extraordinary
    }
}