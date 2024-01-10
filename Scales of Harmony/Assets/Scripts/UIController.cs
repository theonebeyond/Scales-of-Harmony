using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    // UI Elements
    public Text scoreText;
    public GameObject InGameUI;
    public GameObject GameMenu;
    public TraitSelectionPanel TraitSelectionPanel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    public void SwitchMenu(Boolean OnOff)
    {
        GameMenu.gameObject.SetActive(OnOff);
    }

    public void SwitchInGameUI(Boolean OnOff)
    {
        InGameUI.gameObject.SetActive(OnOff);
    }

    public void SwitchTraitSelectionUI(Boolean OnOff, DragonData Dragon)
    {
        if(OnOff)
        {
            TraitSelectionPanel.switchOnOFF(true);
            TraitSelectionPanel.SetDragon(Dragon);
            TraitSelectionPanel.UpdateButtons();
            TraitSelectionPanel.UpdatePics();
        }
        else { TraitSelectionPanel.switchOnOFF(false); }

    }

}
