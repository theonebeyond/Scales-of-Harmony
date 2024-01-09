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
            // ����������˶�������Ч
        }
    }

    private void Die()
    {
        // ��������߼�
        // ���粥������������ֹͣ��ҿ��Ƶ�
        Debug.Log("Player Died");
        // ������Ӹ����߼��������Ϸ
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    // �����Ҫ��������Ӹ��๦�ܣ���������/�����������ֵ����������״̬��
}