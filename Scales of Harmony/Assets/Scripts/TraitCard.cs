using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml.Linq;

public class TraitCard : MonoBehaviour
{
    public TextMeshProUGUI nameText; // Text for displaying the trait's name
    public TextMeshProUGUI descriptionText; // Text for displaying the trait's description
    public Image iconImage; // Image for displaying the trait's icon
    public Image cardBackground; // Image for changing the card's background color based on quality
    public Button cardButton; // Button for player interaction
    private BattleStat battlestat;
    private TraitData traitData; // The data for the trait represented by this card

    // Initialize the card with the given TraitData
    public void InitializeCard(TraitData data)
    {
        traitData = data;

        nameText.text = data.traitName;
        descriptionText.text = data.description;
        iconImage.sprite = data.icon;

        // Set the background color based on the trait's initial quality
        switch (data.initialQuality)
        {
            case TraitData.TraitQuality.Normal:
                nameText.color = Color.white;
                break;
            case TraitData.TraitQuality.Great:
                nameText.color = Color.green;
                break;
            case TraitData.TraitQuality.Excellent:
                nameText.color = Color.blue;
                break;
            case TraitData.TraitQuality.Extraordinary:
                nameText.color = Color.magenta;
                break;
        }

        // Optionally, configure the onClick event of the button
        cardButton.onClick.AddListener(SelectTrait);
    }

    private void SelectTrait()
    {
        if (traitData.segmentPrefab != null)
        {
            // Find the SnakeController and call GrowSnake
            SnakeController snakeController = FindObjectOfType<SnakeController>();
            if (snakeController != null)
            {
                snakeController.GrowSnake(traitData.segmentPrefab);
            }
        }
        battlestat = FindAnyObjectByType<BattleStat>();
        battlestat.AddTrait(traitData);
        UIManager uimanager = FindAnyObjectByType<UIManager>();
        uimanager.UpdateTraitIcon(traitData);
        GameManager gameManager = FindAnyObjectByType<GameManager>();
        gameManager.ChangeState(GameManager.GameState.InGame);
        TraitManager traitManager = FindAnyObjectByType<TraitManager>();
        traitManager.ClearExistingCards();
        traitManager.traitSelectionPanel.gameObject.SetActive(false);

        // Notify TraitManager or other relevant system about the selection
        // FindObjectOfType<TraitManager>().ApplyTrait(traitData);

        // Optionally, hide the trait selection UI after applying the trait
        // This can be done by disabling the parent panel or through a UI manager
    }
}
