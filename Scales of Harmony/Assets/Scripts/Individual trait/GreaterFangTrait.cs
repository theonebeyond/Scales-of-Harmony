public class GreaterFangTrait : Trait
{
    public int attackPowerBonus;

    public GreaterFangTrait()
    {
        name = "Greater Fang";
        description = "Increases attack power";
        traitQuality = Quality.Normal;
        attackPowerBonus = 5; // Example value
    }

    public override void ApplyEffect(SnakeController snakeController, BattleStat battleStat, Quality quality)
    {
        battleStat.UpdateAttackPower(attackPowerBonus * GetQualityMultiplier(quality));
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
