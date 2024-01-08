using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net.Http.Headers;

public class TraitIcon : MonoBehaviour
{
    public TextMeshProUGUI TraitName; // Text for displaying the trait's name
    public Image Icon; // Image for displaying the trait's icon

    private TraitData traitData; // The data for the trait represented by this card

    // Initialize the card with the given TraitData
    public void InitializeIcon(TraitData data )
    {
        traitData = data;

        TraitName.text = data.traitName;
        Icon.sprite = data.icon;

        // Set the background color based on the trait's initial quality
        switch (data.initialQuality)
        {
            case TraitData.TraitQuality.Normal:
                TraitName.color = Color.white;
                break;
            case TraitData.TraitQuality.Great:
                TraitName.color = Color.green;
                break;
            case TraitData.TraitQuality.Excellent:
                TraitName.color = Color.blue;
                break;
            case TraitData.TraitQuality.Extraordinary:
                TraitName.color = Color.magenta;
                break;
        }

        // Optionally, configure the onClick event of the button
    }

    private void SelectTrait()
    {
        // Notify TraitManager or other relevant system about the selection
       // FindObjectOfType<TraitManager>().ApplyTrait(traitData);

        // Optionally, hide the trait selection UI after applying the trait
        // This can be done by disabling the parent panel or through a UI manager
    }
}
