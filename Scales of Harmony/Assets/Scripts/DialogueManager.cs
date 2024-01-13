    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class DialogueManager : MonoBehaviour
{
    // ... other code ...


    public DialogueData Welcome;
    public DialogueData[] Dragons;
    public DialogueData[] MiniBosses;
    public DialogueData FinalBoss;
    public string what;
    private UIController uiController;
    public Button continueButton;
    public enum DialogueState
    {
        None,
        PlayerDialogue,
        EnemyDialogue,
        TransitionToFight
    }

    private DialogueState currentDialogueState = DialogueState.None;
    private void Start()
    {
        uiController = FindAnyObjectByType<UIController>();
    }
    private IEnumerator DisplayDialogue(DialogueData dialogue)
    {
        Debug.Log("DisplayDialogue reached");
        foreach (var line in dialogue.Lines)
        {
            yield return StartCoroutine(DisplayLine(dialogue.TrueTopFalseBottom, line, dialogue.Avatar));
        }

    }

    private IEnumerator DisplayLine(bool TopBottom, string line, Sprite Avatar)
    {
        Debug.Log("DisplayLine reached");
        Debug.Log("TopBottom / Line / Avatar is " + TopBottom + line + Avatar);
        uiController.AppendDialogueTextImage(TopBottom, line, Avatar);

        continueButton.gameObject.SetActive(true); // Show the button

        yield return new WaitForSeconds(1); // Wait for player action

    }


    public void DisPlayWelcomeDialogue() 
    {
        StartCoroutine(DisplayDialogue(Welcome));
    }

    public void DisPlaySaveDragonDialogue(DragonData buddy)
    {
        // Start player dialogue
        DialogueData dragonDialogue = FindDialogueForDragon(buddy);
        StartCoroutine(DisplayDialogue(dragonDialogue)); // Replace 'playerDialogueData' with actual player dialogue data
        currentDialogueState = DialogueState.PlayerDialogue;
    }

    public DialogueData FindDialogueForDragon(DragonData dragonData)
    {
        foreach (DialogueData dialogue in Dragons)
        {
            if (dialogue.DialogueSpeaker == dragonData.dragonName)
            {
                return dialogue;
            }
        }
        return null; // Return null if no matching dialogue is found
    }
    public void DisPlayMiniBossDialogue()
    {

    }


    public void DisPlayFinalBossDialogue()
    {

    }
    // ... other methods ...
}