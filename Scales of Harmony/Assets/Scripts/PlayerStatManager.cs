using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // 可以添加受伤动画或音效
        }
    }

    private void Die()
    {
        // 玩家死亡逻辑
        // 比如播放死亡动画，停止玩家控制等
        Debug.Log("Player Died");
        // 可以添加复活逻辑或结束游戏
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    // 如果需要，可以添加更多功能，比如增加/减少最大生命值、处理特殊状态等
}