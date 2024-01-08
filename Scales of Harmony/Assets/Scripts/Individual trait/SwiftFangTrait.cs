public class SwiftFangTrait : Trait
{
    public float doubleAttackChance; // Percentage

    public SwiftFangTrait()
    {
        name = "Swift Fang";
        description = "Chance of double attack";
        traitQuality = Quality.Normal;
        doubleAttackChance = 10; // Example value
    }

    public override void ApplyEffect(SnakeController snakeController, BattleStat battleStat, Quality quality)
    {
        // Apply the chance of double attack, possibly affecting battle mechanics
    }
}