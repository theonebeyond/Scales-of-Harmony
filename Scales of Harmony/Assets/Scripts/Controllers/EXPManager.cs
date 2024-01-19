using UnityEngine;

public class EXPManager : MonoBehaviour
{
    public int currentExp = 0;
    public int expToNextLevel = 3;
    public UIController uiManager;
    public PlayerStatManager playerStatManager;

    void Start()
    {
        uiManager.UpdateExpUI(currentExp, expToNextLevel);
    }

    public void GainExp(int amount)
    {
        currentExp += amount;
        uiManager.UpdateExpUI(currentExp, expToNextLevel);
        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
        //uiManager.UpdateExpUI(currentExp, expToNextLevel);
    }

    private void LevelUp()
    {
        currentExp -= expToNextLevel;
        expToNextLevel++;
        uiManager.UpdateExpUI(currentExp, expToNextLevel);
        playerStatManager.LevelUP();
        playerStatManager.FullHealth();

    // Increase expToNextLevel for the next level, adjust stats, etc.
    // Handle leveling up logic
}
}