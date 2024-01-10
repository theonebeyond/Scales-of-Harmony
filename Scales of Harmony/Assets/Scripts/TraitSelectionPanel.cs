using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TraitSelectionPanel : MonoBehaviour
{
    public DragonData dragon; // Assign in the inspector
    //public GameObject panel;
    public Button[] traitButtons; // Assign in the inspector
    public UnityEngine.Object[] traitPics; // Assign in the inspector
    public int playerchoice;
    public PowerManager powerManager;
    public GameManager gameManager;
    //private int currentTraitIndex = 0;  

    void Start()
    {
       powerManager = FindAnyObjectByType<PowerManager>();
       gameManager = FindAnyObjectByType<GameManager>();
    }

    public void UpdateButtons()
    {
        for (int i = 0; i < traitButtons.Length; i++)
        {
            // Empty slot
            Debug.Log("now im writing button text and now is" + i);
                traitButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = dragon.blessings[i].blessingName;
                traitButtons[i].interactable = true;
            
        }
    }

    public void UpdatePics()
    {
        for (int i = 0; i < traitPics.Length; i++)
        {
            Image imageComponent = traitPics[i].GetComponent<Image>() as Image;
            if (imageComponent != null)
            {
                imageComponent.sprite = dragon.blessings[i].blessingSprite;
            }
        }
    }
    public void switchOnOFF(Boolean OnOff)
    {
        gameObject.SetActive(OnOff);

    }

    public void SetDragon(DragonData Dragon)
    {
        this.dragon = Dragon;
    }

    public void AddTrait(int number)
    {
      // Extract the blessingNo and pass to powerManager to add trait into current trait list
        powerManager.AddBlessing(dragon.blessings[number].blessingNo);
        gameManager.ResumeGame();
    }
    /*
    public void SelectTrait(int blessingIndex)
    {
        if (currentTraitIndex < dragon.blessings.Length)
        {
            // Assign the selected blessing to the current trait slot
            dragon.blessings[currentTraitIndex] = // retrieve the selected Blessing object
            currentTraitIndex++;
            UpdateButtons();
        }

        // Perform additional logic like closing the panel, applying trait effects, etc.
    }*/
}
