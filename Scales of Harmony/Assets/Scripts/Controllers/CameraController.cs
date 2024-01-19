using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CameraFollow cameraFollow; // Assign in inspector
    public CameraMover cameraMover;
    public CameraMoverDragon Dragon;// Assign in inspector
    public GameManager gameManager;
    public UIController uIController;
    public DialogueManager dialogueManager;

    private void Start()
    {
        // Starting with the camera following the player
        EnableFollow();
        uIController = FindAnyObjectByType<UIController>();
        gameManager = FindAnyObjectByType<GameManager>();
        dialogueManager = FindAnyObjectByType<DialogueManager>();
    }

    public void EnableFollow()
    {
        cameraFollow.enabled = true;
        cameraMover.enabled = false;
        Dragon.enabled = false;
    }

    public void EnableMoveToFixedPosition()
    {
        cameraFollow.enabled = false;
        cameraMover.enabled = true;
        Dragon.enabled = false;
    }
    public void EnableMoveToDragon()
    {
        cameraFollow.enabled = false;
        cameraMover.enabled = false;
        Dragon.enabled = true;
    }
    public void ReachedPoint() {
        cameraMover.enabled = false;
        DragonData dragon = gameManager.GetCurrentDragon();

        uIController.PlayMeetBoss(dragon);
        Debug.Log("I reached here: thispos == targetpos and next step is to displayminiboss");
        BOSSAI bOSSAI = FindAnyObjectByType<BOSSAI>();
        if (bOSSAI.bossData.IsFinal)
            StartCoroutine(dialogueManager.DisPlayMiniBossDialogue());
        else
        { StartCoroutine(dialogueManager.DisPlayMiniBossDialogue()); }


    }


    public void ReachedPointDragon() 
    {
        Dragon.enabled = false;
        uIController.PlayMeetFriend(gameManager.GetCurrentDragon());
        gameManager.DialogueUISetting();

    }


    public void ResetBool()
    {
        cameraMover.ResetBool();
        Dragon.ResetBool();
    }

}