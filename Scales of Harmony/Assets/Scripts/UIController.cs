using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    // UI Elements
    public Slider healthSlider;
    public Slider EXPSlider;
    public TextMeshProUGUI scoreText;
    public int score = 0;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI EXPText;
    public GameObject InGameUI;
    public GameObject GameMenu;
    public GameObject GameOverMenu;
    public GameObject MenuButton;
    public GameObject WelcomeTXT;
    public TextMeshProUGUI WelcomeText;
    public TraitSelectionPanel TraitSelectionPanel;
    public GameObject DialogueBox;
    public TextMeshProUGUI DialogueTopText;
    public TextMeshProUGUI DialogueBottomText;
    public GameObject TopDialogueBox;
    public GameObject BottomDialogueBox;
    public GameObject ImageTopText;
    public GameObject ImageBottomText;
    public GameObject AttributeMenu;
    public GameObject backgroundBase;
    //below are all AttributeText
    public GameObject[] Attribute;
    public GameObject AttributePoint;
    public float letterDelay = 0.03f; // Delay between letters, can be adjusted


    /*
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    */

    public void SwitchMenu(Boolean OnOff)
    {
        GameMenu.gameObject.SetActive(OnOff);
    }

    public void SwitchInGameUI(Boolean OnOff)
    {
        InGameUI.gameObject.SetActive(OnOff);
    }

    public void SwitchSettingButton(Boolean OnOff)
    {
        MenuButton.gameObject.SetActive(OnOff);
    }

    public void SwitchDialogueBox(Boolean OnOff)
    {
        DialogueBox.gameObject.SetActive(OnOff);
    }
    public void WelcomeTXTSwitch(Boolean OnOff) {
        WelcomeTXT.SetActive(OnOff);
    }

    public void UpdateScore()
    {
        score++;
        scoreText.text = "Dark Force before next boss: " + (100-score).ToString();

    }
    public void DisplayGameOverScreen(Boolean OnOff)
    {
        GameOverMenu.gameObject.SetActive(OnOff);
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        healthText.text = $"Health: {currentHealth}/{maxHealth}";
        healthSlider.value = (float)currentHealth / maxHealth;
    }

    public void UpdateExpUI(int currentExp, int nextLevelExp)
    {
        EXPText.text = $"EXP: {currentExp}/{nextLevelExp}";
        EXPSlider.value = (float)currentExp / nextLevelExp;
    }
    public void SwitchTraitSelectionUI(Boolean OnOff, DragonData Dragon)
    {
        if (OnOff)
        {
            TraitSelectionPanel.switchOnOFF(true);
            TraitSelectionPanel.SetDragon(Dragon);
            TraitSelectionPanel.UpdateButtons();
            TraitSelectionPanel.UpdatePics();
        }
        else { TraitSelectionPanel.switchOnOFF(false); }

    }

    public void SetAttribute(Dictionary<string, AttributeData> attributeData, int AttributeLeft)
    {
        for (int i = 0; i < Attribute.Length; i++)
        {
            // For each parent, iterate through its children
            foreach (Transform child in Attribute[i].transform)
            {
                // For each child, iterate through its children (grandchildren)
                // Assign values based on tags
                TextMeshProUGUI textMesh = child.GetComponent<TextMeshProUGUI>();
                if (child.tag == "Attribute_Before" || child.tag == "Attribute_After")
                {

                    if (textMesh != null)
                    {

                        textMesh.text = attributeData[attributeData.ElementAt(i).Key].CurrentValue.ToString(); // Replace with the value you want to assign
                        textMesh.color = Color.white;
                    }
                }
                if (child.tag == "Attribute_Applied")
                {
                    if (textMesh != null)
                    {
                        textMesh.text = "0";
                    }
                }

                if (child.tag == "Attribute_Incremental")
                {
                    if (textMesh != null)
                    {
                        textMesh.text = "(" + attributeData[attributeData.ElementAt(i).Key].IncrementalValue.ToString() + ")";
                    }
                }
            }
            InGameAttributePointUpdate(AttributeLeft);
        }
        AttributeLeftSwitch(true);
        AttributeMenuSwitch(true);
    }

    public void UIAddButtonAttribute(string attributeName, int updatedValue, GameObject Attribute, int AttributeLeft)
    {
        foreach (Transform child in Attribute.transform)
        {
            TextMeshProUGUI textMesh = child.GetComponent<TextMeshProUGUI>();
            // Check which object to be modified
            if (textMesh != null && child.tag == "Attribute_After")
            {
                textMesh.text = updatedValue.ToString();
                textMesh.color = Color.blue;

            }
            if (textMesh != null && child.tag == "Attribute_Applied")
            {
                if (int.TryParse(textMesh.text, out int currentValue))
                {
                    currentValue++; // Increment the value
                    textMesh.text = currentValue.ToString(); // Update the text
                }// Applied count++
            }

        }
        InGameAttributePointUpdate(AttributeLeft);

    }


    public void AttributeMenuSwitch(Boolean OnOff) {

        AttributeMenu.SetActive(OnOff);
    }
    public void AttributeLeftSwitch(Boolean OnOff)
    {

        AttributePoint.SetActive(OnOff);
    }

    public void InGameAttributePointUpdate(int AttributeLeft) {

        TextMeshProUGUI attpoint = AttributePoint.GetComponent<TextMeshProUGUI>();
        if (attpoint != null)
        {
            attpoint.text = AttributeLeft.ToString(); // Update the text
        }// Applied count++
    }
    public void AttributePointAddSwitch(Boolean OnOff)
    {

        for (int i = 0; i < Attribute.Length; i++)
        {
            // For each parent, iterate through its children
            foreach (Transform child in Attribute[i].transform)
            {
                // For each child, iterate through its children (grandchildren)

                // Here you can assign values based on the grandchild's name
                // For example, if the grandchild's name is "aa"
                if (child.tag == "Attribute_Button")
                {
                    if (child != null)
                    {
                        child.gameObject.SetActive(OnOff);
                    }
                }
                // Add more conditions for other grandchildren as needed
            }
        }
    }

    public void AppendDialogueTextImage(bool TopBottom, string text, Sprite dialogueImage)
    {
        GameObject targetDialogueBox = TopBottom ? TopDialogueBox : BottomDialogueBox;
        GameObject closeDialogueBox = TopBottom ? BottomDialogueBox : TopDialogueBox;
        TextMeshProUGUI targetTextComponent = TopBottom ? DialogueTopText : DialogueBottomText;
        Image targetImageComponent = TopBottom ? ImageTopText.GetComponent<Image>() : ImageBottomText.GetComponent<Image>();

        targetDialogueBox.SetActive(true); // Activate the correct dialogue box
        closeDialogueBox.SetActive(false); // Deactivate the wrong dialogue box
        targetImageComponent.sprite = dialogueImage; // Set the new sprite
        targetTextComponent.text = "";
        Debug.Log("text to print is: " + text);
        StartCoroutine(AppendTextCoroutine(targetTextComponent, text)); // Start appending text
    }

    private IEnumerator AppendTextCoroutine(TextMeshProUGUI textComponent, string text)
    {
        foreach (char letter in text)
        {
            Debug.Log("I'm writting this letter: " + letter);
            textComponent.text += letter; // Append one character at a time
            yield return new WaitForSeconds(letterDelay); // Adjust the delay as needed
        }
    }
    public void printWelcome() 
    {
        string welcomeText = WelcomeText.text;
        WelcomeText.text = "";
        StartCoroutine(AppendTextCoroutine(WelcomeText, welcomeText));

    }
    public void TopDialogueBoxSwitch(bool OnOff)
    {
        TopDialogueBox.SetActive(OnOff);
    }

    public void BottomDialogueBoxSwitch(bool OnOff)
    {
        BottomDialogueBox.SetActive(OnOff);
    }

    public void DialogueBoxSwitch(bool OnOff)
    {
        DialogueBox.SetActive(OnOff);
    }
}
