using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dragon", menuName = "Dragon")]
public class DragonData : ScriptableObject
{
    public string dragonName;
    public Sprite icon;
    public Sprite dragonSprite;
    public Blessing[] blessings = new Blessing[3];
    // Add any other relevant properties for the Dragon.
}