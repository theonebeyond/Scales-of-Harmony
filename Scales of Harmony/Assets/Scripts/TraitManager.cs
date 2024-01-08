using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TraitManager : MonoBehaviour
{
    public GameObject traitCardPrefab;
    public Transform traitSelectionPanel;
    public int playerLevel = 1; // Should be set based on the player's actual level
    private List<TraitData> availableTraits; // All available traits
    private HashSet<string> extraordinaryTraits; // Tracks extraordinary traits acquired
    EXPManager EXPManager;

    void Start()
    {
        EXPManager = FindObjectOfType<EXPManager>();
        InitializeTraits();
        extraordinaryTraits = new HashSet<string>();
    }

    private void InitializeTraits()
    {
        TraitData[] traits = Resources.LoadAll<TraitData>("Traits");
        availableTraits = new List<TraitData>(traits);
        foreach (TraitData trait in availableTraits)
        {
            Debug.Log("Loaded Trait: " + trait.traitName);
        }
    }

    public void OfferTraitChoices()
    {
        Debug.Log("reached OfferTraitChoices");
        ClearExistingCards();

        List<TraitData> chosenTraits = new List<TraitData>();

        for (int i = 0; i < 3; i++)
        {
            TraitData chosenTrait = ChooseUniqueTraitForPlayer(chosenTraits);
            if (chosenTrait != null)
            {
                chosenTraits.Add(chosenTrait);
                GameObject cardObj = Instantiate(traitCardPrefab, traitSelectionPanel);
                TraitCard cardScript = cardObj.GetComponent<TraitCard>();
                cardScript.InitializeCard(chosenTrait);
            }
        }

        traitSelectionPanel.gameObject.SetActive(true);
    }

    private TraitData ChooseUniqueTraitForPlayer(List<TraitData> chosenTraits)
    {
        playerLevel = EXPManager.currentLevel;
        List<TraitData> validTraits = availableTraits
            .Where(trait => trait.minLevel <= playerLevel && !extraordinaryTraits.Contains(trait.traitName) && !chosenTraits.Contains(trait))
            .ToList();

        if (validTraits.Count == 0)
        {
            return null; // No valid traits to choose from
        }

        return SelectTraitBasedOnRatios(validTraits);
    }


    private TraitData SelectTraitBasedOnRatios(List<TraitData> traits)
    {
        float roll = Random.Range(0f, 100f);
        TraitData.TraitQuality quality;

        // Determine quality based on provided ratios
        if (roll < 50) quality = TraitData.TraitQuality.Normal;
        else if (roll < 80) quality = TraitData.TraitQuality.Great;
        else if (roll < 95) quality = TraitData.TraitQuality.Excellent;
        else quality = TraitData.TraitQuality.Extraordinary;

        // Filter traits by quality and select randomly
        List<TraitData> filteredTraits = traits.Where(trait => trait.initialQuality <= quality).ToList();
        return filteredTraits[Random.Range(0, filteredTraits.Count)];
    }

    public void ClearExistingCards()
    {
        foreach (Transform child in traitSelectionPanel)
        {
            Destroy(child.gameObject);
        }
    }

    public void AddExtraordinaryTrait(string traitName)
    {
        extraordinaryTraits.Add(traitName);
    }

    // Other methods as needed...
}