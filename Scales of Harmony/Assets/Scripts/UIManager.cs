using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI levelText;
    public Text bodyCountText;
    public Slider xpBar;
    public GameObject TraitIcon;
    public Transform TraitDisplayPanel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateTimer(float timer)
    {
        TimerText.text = "Time Left: " + timer.ToString();
    }
    public void UpdateLevelDisplay(int level)
    {
        levelText.text = "Level: " + level;
    }

    public void UpdateTraitIcon(TraitData traitdata) {
        GameObject IconObj = Instantiate(TraitIcon, TraitDisplayPanel);
        TraitIcon IconScript = IconObj.GetComponent<TraitIcon>();
        IconScript.InitializeIcon(traitdata);

    }

    public void SwitchOfTraitDisplay(Boolean OnOff)
    {
        TraitDisplayPanel.gameObject.SetActive(OnOff);
    }


    public void UpdateBodyCount(int bodyCount)
    {
        bodyCountText.text = "Bodies: " + bodyCount;
    }

    public void UpdateXPBar(int currentXP, int nextLevelXP)
    {
        xpBar.maxValue = nextLevelXP;
        xpBar.value = currentXP;
        TextMeshProUGUI sliderText = xpBar.GetComponentInChildren<TextMeshProUGUI>();
        sliderText.text = currentXP + "/" + nextLevelXP;

    }

    public void SwitchMainDisplay(Boolean OnOff)
    {
        TimerText.gameObject.SetActive(OnOff);
        levelText.gameObject.SetActive(OnOff);
        xpBar.gameObject.SetActive(OnOff);
    }
}