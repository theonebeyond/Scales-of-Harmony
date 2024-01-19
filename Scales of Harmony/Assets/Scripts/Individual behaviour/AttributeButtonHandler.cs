using UnityEngine;
using UnityEngine.UI;

public class AttributeButtonHandler : MonoBehaviour
{
    public Button attributeButton; // Assign in Inspector
    public string attributeName;   // Assign in Inspector
                                   // Set default increment value, can be adjusted in Inspector

    private PlayerStatManager playerStatManager;
    private GameObject attributeParent;

    void Start()
    {
        playerStatManager = FindAnyObjectByType<PlayerStatManager>();// get reference to PlayerStatManager, e.g., FindObjectOfType<PlayerStatManager>();
        attributeParent = this.gameObject.transform.parent.gameObject; // Assuming the script is attached to the button

        attributeButton.onClick.AddListener(() => playerStatManager.PSMAddButtonAttribute(attributeName, attributeParent));
    }
}