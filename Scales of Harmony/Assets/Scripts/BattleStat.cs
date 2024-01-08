using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleStat : MonoBehaviour
{
    public int health = 100;
    public int attackPower = 10;
    public int DamageBlock = 0;
    public int DoubleAttackChance = 0; // Percentage
    public int dodgeChance = 0; // Percentage
    private List<TraitData> currentTraits = new List<TraitData>();
    private List<TraitData> StatTraits = new List<TraitData>();
    private List<TraitData> PassiveTraits = new List<TraitData>();
    private List<TraitData> ActiveTraits = new List<TraitData>();
    public int currentPlayerLevel = 1;
    public int currentGameLevel = 1;

    public static BattleStat Instance { get; private set; }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void UpdateHealth(int amount)
    {
        health += amount;
        // Implement health limit logic here
    }

    public void UpdateAttackPower(int amount)
    {
        attackPower += amount;
        // Implement attack power logic here
    }

    public void AddTrait(TraitData trait)
    {

        if (trait.TraitType.Equals(1)) {
            ActiveTraits.Add(trait);
        }
        if (trait.TraitType.Equals(2)) {
            StatTraits.Add(trait);
        }
        if (trait.TraitType.Equals(3)) {
            PassiveTraits.Add(trait);
        }
        // Check if we already have this trait and upgrade its quality if so
        if (currentTraits.Exists(t => t.traitName == trait.traitName))
        {
            TraitData existingTrait = currentTraits.Find(t => t.traitName == trait.traitName);
            if (existingTrait.initialQuality < TraitData.TraitQuality.Extraordinary)
            {
                existingTrait.initialQuality += 1; // Upgrade quality
            }
        }
        else
        {
            currentTraits.Add(trait);
        }

        // Implement any other logic needed when a new trait is added
    }



    public void IncreaseDodgeChance(int chance)
    {
        dodgeChance += chance;
        // Implement dodge chance logic here
    }

    public void AddDamageBlock(int amount)
    {
        DamageBlock += amount;

    }


    public void UpdatePlayerLevel(int level)
    {
        currentPlayerLevel = level;
    }

    public void UpdateGameLevel()
    {
        currentGameLevel++;
    }


    public List<TraitData> GetCurrentTraits()
    {
        return currentTraits;
    }
    // Add more functions as needed for managing passive and active traits
}