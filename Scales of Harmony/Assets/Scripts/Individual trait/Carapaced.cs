public class CarapacedTrait : Trait
{
    public int damageBlockAmount;

    public CarapacedTrait()
    {
        name = "Carapaced";
        description = "Blocks a certain amount of damage in boss fights";
        traitQuality = Quality.Normal;
        damageBlockAmount = 10; // Example value
    }

    public override void ApplyEffect(SnakeController snakeController, BattleStat battleStat, Quality quality)
    {
        battleStat.AddDamageBlock(damageBlockAmount * GetQualityMultiplier(quality));
        // Implement AddDamageBlock in BattleStat to handle this
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
