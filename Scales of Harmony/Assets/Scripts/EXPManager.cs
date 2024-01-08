using UnityEngine;

public class EXPManager : MonoBehaviour
{
    public int currentLevel = 1;
    public int currentXP = 0;
    public int nextLevelXP = 100;
    public static EXPManager Instance { get; private set; }
    GameManager gameManager;
    BattleStat battleStat;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        battleStat = FindObjectOfType<BattleStat>();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddXP(int amount)
    {
        currentXP += amount;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (currentXP >= nextLevelXP)
        {
            currentXP -= nextLevelXP;
            currentLevel++;
            battleStat.UpdatePlayerLevel(currentLevel);
            nextLevelXP = CalculateNextLevelXP();
            gameManager.PlayerLeveledUp();
            // Notify the UIManager to update the UI

        }
        UIManager.Instance.UpdateLevelDisplay(currentLevel);
        UIManager.Instance.UpdateXPBar(currentXP, nextLevelXP);
    }

    private int CalculateNextLevelXP()
    {
        // You can adjust the formula as needed
        return nextLevelXP + 200 * currentLevel;
    }
}