using UnityEditor;
using UnityEngine;

public class FieryScaleTrait : Trait
{
    public TraitData traitData; // Reference to the Scriptable Object
    public FieryScaleTrait()
    {
        name = "Fiery Scale";
        description = "Adds a fireball-casting body segment in boss fights";
        traitQuality = Quality.Normal;
    }

    public override void ApplyEffect(SnakeController snakeController, BattleStat battleStat, Quality quality)
    {
        if (traitData != null && traitData.segmentPrefab != null)
        {
            snakeController.GrowSnake(traitData.segmentPrefab);
        }
    }

    private int GetQualityMultiplier(Quality quality)
    {
        // Return a multiplier based on the quality
        switch (quality)
        {
            case Quality.Great:
                return 2;
            case Quality.Excellent:
                return 3;
            case Quality.Extraordinary:
                return 4;
            default:
                return 1;
        }
    }

}
