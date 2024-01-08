public abstract class Trait
{
    public string name;
    public string description;
    public enum TraitType { Active, Passive }
    public TraitType type;
    public enum Quality { Normal, Great, Excellent, Extraordinary }
    public Quality traitQuality;

    // Existing properties and methods...

    public abstract void ApplyEffect(SnakeController snakeController, BattleStat battleStat, Quality quality);

    // Method to upgrade the quality of the trait
    public void UpgradeQuality()
    {
        if (traitQuality < Quality.Extraordinary)
        {
            traitQuality++;
        }
    }

}