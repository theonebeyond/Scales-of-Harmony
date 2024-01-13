public class AttributeData
{
    public int CurrentValue;
    public int UpdatedValue;
    public int IncrementalValue;

    public AttributeData(int currentValue, int incrementalValue)
    {
        CurrentValue = currentValue;
        UpdatedValue = currentValue;
        IncrementalValue = incrementalValue;
    }
}
