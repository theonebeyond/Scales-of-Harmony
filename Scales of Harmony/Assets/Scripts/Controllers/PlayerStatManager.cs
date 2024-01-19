using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;
using TMPro;
using System;
using System.Collections;

public class PlayerStatManager : MonoBehaviour
{
    public int currentHealth;
    public int[] MaxHealth;
    public int[] AttackPower;
    public int[] AttackSpeed;
    public int[] Range;
    public int[] Armor;
    public int[] CritChance;
    public int[] Dodge;
    public Canvas canvas;
    public GameObject DodgePrefab;
    public Button[] yourButton;
    private UIController uIController;
    private GameManager gameManager;
    private Dictionary<string, AttributeData> attributes = new Dictionary<string, AttributeData>();
    public int AttributePoint;
    private int AttributeLeft;
    public int score = 0;
    public int mobUntilNextWave;
    //public static PlayerStatManager Instance { get; private set; }

    /*
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: If you want to persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    */

    void Start()
    {
        AttributeLeft = AttributePoint;
        uIController = FindAnyObjectByType<UIController>();
        gameManager = FindAnyObjectByType<GameManager>();
        currentHealth = MaxHealth[0];
        uIController.UpdateHealthUI(currentHealth, MaxHealth[0]);
        InitializeAttributes();
    }

    private void InitializeAttributes()
    {
        attributes.Add("MaxHealth", new AttributeData(MaxHealth[0], MaxHealth[1]));
        attributes.Add("AttackPower", new AttributeData(AttackPower[0], AttackPower[1]));
        attributes.Add("AttackSpeed", new AttributeData(AttackSpeed[0], AttackSpeed[1]));
        attributes.Add("Range", new AttributeData(Range[0], Range[1]));
        attributes.Add("Armor", new AttributeData(Armor[0], Armor[1]));
        attributes.Add("CritChance", new AttributeData(CritChance[0], CritChance[1]));
        attributes.Add("Dodge", new AttributeData(Dodge[0], Dodge[1]));

        // Update UI with initial attribute values
    }

    public void PSMAddButtonAttribute(string attributeName, GameObject Attribute)
    {
        AttributeLeft--;
        if (attributes.ContainsKey(attributeName))
        {
            attributes[attributeName].UpdatedValue += attributes[attributeName].IncrementalValue;

            PSMAddButtonAttributeUI(attributeName, attributes[attributeName].UpdatedValue, Attribute);
        }
        CheckAttributePoint();

    }

    public void PSMAddButtonAttributeUI(string attributeName, int updatedValue, GameObject Attribute)
    {
        // Update the UI element corresponding to the attribute
        // This depends on your UI setup
        uIController.UIAddButtonAttribute(attributeName, updatedValue, Attribute, AttributeLeft);
    }
    public void TakeDamage(int damage)
    {
        Debug.Log("If not dodge, I'm taking " + damage + " damage");
        System.Random random = new System.Random();
        int randomValue = random.Next(1, 101);
        if (randomValue >= this.Dodge[0])
        {
            float modified = (float)damage * (1 - ((float)this.Armor[0] / 100));
            int intmodified = (int)modified;
            Debug.Log("float modified damage is " + modified + "should've taken " + damage + "damage, but got armor so actually took" + intmodified + "damage. modifier is " + (1 - ((float)this.Armor[0] / 100)));
            currentHealth -= intmodified;
            // Maybe an animation/soundFX here
        }
        if (randomValue <= this.Dodge[0])
        {
            Debug.Log("Just dodged an attack with chance " + this.Dodge[0] + " and randomvalue is " + randomValue);
            SpawnFloatingText(this.transform.position,"Dodged!");
            // A dodge text generated
        }
        if (currentHealth <= 0) { currentHealth = 0; Die(); }
        uIController.UpdateHealthUI(currentHealth, MaxHealth[0]);

    }

    public void FullHealth() {
        currentHealth = MaxHealth[0];
        uIController.UpdateHealthUI(currentHealth, MaxHealth[0]);
    }

    public void ProjectileHit(int damage)
    {
        TakeDamage(damage);


    }
    private void Die()
    {
        // Player Die logic
        Debug.Log("Player Died");
        gameManager.GameOver();
        // Maybe game over called here
    }
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth[0]);
    }

    public void Initialize_Attribute_Board()
    {

        uIController.SetAttribute(attributes, AttributeLeft);
        CheckAttributePoint();

    }

    public void CheckAttributePoint() {

        if (AttributeLeft == 0)
        {
            uIController.AttributePointAddSwitch(false);
        }
        if (AttributeLeft > 0)
        {
            uIController.AttributePointAddSwitch(true);
        }

    }

    public void AttributeDone()
    {
        AttributeLeft = AttributePoint;
        if (AttributePoint == 0)
        {
            uIController.AttributeLeftSwitch(false);
        }
        if (AttributePoint > 0)
        {
            uIController.AttributeLeftSwitch(true);
            uIController.InGameAttributePointUpdate(AttributeLeft);
        }

    }
    public void UndoAttributeChanges() {

        foreach (var attribute in attributes)
        {
            attribute.Value.UpdatedValue = attribute.Value.CurrentValue;

            // Apply changes to actual player stats here
            // For example:
            switch (attribute.Key)
            {
                case "MaxHealth":
                    MaxHealth[0] = attribute.Value.CurrentValue;
                    break;
                case "AttackPower":
                    AttackPower[0] = attribute.Value.CurrentValue;
                    break;
                case "AttackSpeed":
                    AttackSpeed[0] = attribute.Value.CurrentValue;
                    break;
                case "Armor":
                    Armor[0] = attribute.Value.CurrentValue;
                    break;
                case "CritChance":
                    CritChance[0] = attribute.Value.CurrentValue;
                    break;
                case "Range":
                    Range[0] = attribute.Value.CurrentValue;
                    break;
                case "Dodge":
                    Dodge[0] = attribute.Value.CurrentValue;
                    break;
                    // Add cases for other attributes
            }


        }
        // Optionally, call a method to update the UI after applying changes
        AttributeLeft = AttributePoint;
        Initialize_Attribute_Board();
    }


    public void ApplyAttributeChanges()
    {
        foreach (var attribute in attributes)
        {
            attribute.Value.CurrentValue = attribute.Value.UpdatedValue;

            // Apply changes to actual player stats here
            // For example:
            switch (attribute.Key)
            {
                case "MaxHealth":
                    MaxHealth[0] = attribute.Value.CurrentValue;
                    break;
                case "AttackPower":
                    AttackPower[0] = attribute.Value.CurrentValue;
                    break;
                case "AttackSpeed":
                    AttackSpeed[0] = attribute.Value.CurrentValue;
                    break;
                case "Armor":
                    Armor[0] = attribute.Value.CurrentValue;
                    break;
                case "CritChance":
                    CritChance[0] = attribute.Value.CurrentValue;
                    break;
                case "Range":
                    Range[0] = attribute.Value.CurrentValue;
                    break;
                case "Dodge":
                    Dodge[0] = attribute.Value.CurrentValue;
                    break;
                    // Add cases for other attributes
            }
        }

        uIController.UpdateHealthUI(currentHealth, MaxHealth[0]);
        AttributePoint = AttributeLeft;
        // Optionally, call a method to update the UI after applying changes
        Initialize_Attribute_Board();
    }


    public void LevelUP() {
        AttributePoint++;
        AttributeDone();
    }

    public void ScoreUp() {
        score++;
        int ScoreLeft = mobUntilNextWave - score;
        uIController.UpdateScore(ScoreLeft);
        if (score >= mobUntilNextWave) 
        {
            score = 0;
            uIController.UpdateScore(ScoreLeft);
            gameManager.EnterBossPhase();

        }
    }

    public void SpawnFloatingText(Vector2 worldPosition, string text)
    {

        GameObject textObj = Instantiate(DodgePrefab, canvas.transform);
        textObj.GetComponent<TextMeshProUGUI>().text = text;
        Vector3 position = textObj.transform.position;
        position.y += 80;
        position.x += 27;
        textObj.transform.position = position;
    }
}

