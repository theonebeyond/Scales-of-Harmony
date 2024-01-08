public class GrowBiggerTrait : Trait
{
    public int healthBonus;

    public GrowBiggerTrait()
    {
        name = "Grow Bigger";
        description = "Increases maximum health";
        traitQuality = Quality.Normal;
        healthBonus = 20; // Example value, can be scaled with quality
    }

    public override void ApplyEffect(SnakeController snakeController, BattleStat battleStat, Quality quality)
    {
        // Increase health considering trait quality
        battleStat.UpdateHealth(healthBonus * GetQualityMultiplier(quality));
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
