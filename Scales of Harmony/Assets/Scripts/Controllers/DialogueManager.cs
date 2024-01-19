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
    public DialogueData[] PlayerMiniBosses;
    public DialogueData FinalBoss;
    public DialogueData FinalPlayer;
    public string what;
    private UIController uiController;
    public GameManager gameManager;
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
        uiController.AppendDialogueTextImage(TopBottom, line, Avatar); // Show the button

        yield return new WaitForSeconds(1); // Wait for player action

    }


    public void DisPlayWelcomeDialogue() 
    {
        uiController.DialogueButtonWelcomeSwitch(true);
        uiController.DialogueButtonDragonSwitch(false);
        uiController.DialogueButtonBossSwitch(false);

        StartCoroutine(DisplayDialogue(Welcome));
    }

    public IEnumerator DisPlaySaveDragonDialogue(DragonData buddy)
    {
        // Start player dialogue
        uiController.DialogueButtonWelcomeSwitch(false);
        uiController.DialogueButtonDragonSwitch(false);
        uiController.DialogueButtonBossSwitch(false);
        DialogueData dragonDialogue = FindDialogueForDragon(buddy);
        yield return StartCoroutine(DisplayDialogue(dragonDialogue)); // Replace 'playerDialogueData' with actual player dialogue data
        yield return new WaitForSeconds(5);
        uiController.DialogueButtonDragonSwitch(true);
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
    public IEnumerator DisPlayMiniBossDialogue()
    {
        uiController.DialogueButtonWelcomeSwitch(false);
        uiController.DialogueButtonDragonSwitch(false);
        Debug.Log("I reached here: DisPlayMiniBossDialogue");
        int randomIndexP = Random.Range(0, PlayerMiniBosses.Length);
        DialogueData PlayerMiniDialogue = PlayerMiniBosses[randomIndexP];
        int randomIndexE = Random.Range(0, MiniBosses.Length);
        DialogueData EnemyMiniDialogue = MiniBosses[randomIndexE];
        yield return StartCoroutine(DisplayDialogue(PlayerMiniDialogue));
        Debug.Log("I reached here: after called startcor.... playerminidialog");
        // Wait for 1 second
        yield return new WaitForSeconds(4);

        // Start the EnemyDialogue coroutine
        yield return StartCoroutine(DisplayDialogue(EnemyMiniDialogue));
        Debug.Log("I reached here: after starcorou... DisPlayMiniBossDialogue");
        yield return new WaitForSeconds(4);
        uiController.DialogueButtonBossSwitch(true);
    }


    public IEnumerator DisPlayFinalBossDialogue()
    {
        uiController.DialogueButtonWelcomeSwitch(false);
        uiController.DialogueButtonDragonSwitch(false);
        Debug.Log("I reached here: DisPlayMiniBossDialogue");
        yield return StartCoroutine(DisplayDialogue(FinalPlayer));
        Debug.Log("I reached here: after called startcor.... playerminidialog");
        // Wait for 1 second
        yield return new WaitForSeconds(3);

        // Start the EnemyDialogue coroutine
        yield return StartCoroutine(DisplayDialogue(FinalBoss));
        Debug.Log("I reached here: after starcorou... DisPlayMiniBossDialogue");
        yield return new WaitForSeconds(3);
        uiController.DialogueButtonBossSwitch(true);
    }
    // ... other methods ...
}