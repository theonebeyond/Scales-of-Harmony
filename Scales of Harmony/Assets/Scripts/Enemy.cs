using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public int attackPower = 20;
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private BattleManager battleManager;
    private BattleStat battleStat;

    void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
        battleStat = FindObjectOfType<BattleStat>();
        Debug.Log("Let me let me let me start the enenene  ennnnneeeemmmmyyyyyy");
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public void SetupBattle()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[battleStat.currentGameLevel - 1];
        Debug.Log("finished setupenemy battle");
    }

    public IEnumerator ExecuteTurn()
    {
        PerformAttack();
        yield return new WaitForSeconds(1); // Add delay for the attack, adjust as needed
    }

    private void PerformAttack()
    {
        battleManager.player.TakeDamage(attackPower);
        Debug.Log("Enemy had performed an attack");
    }

    private void Die()
    {
        battleManager.EnemyDefeated();
    }
}
