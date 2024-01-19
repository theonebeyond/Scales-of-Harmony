using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Transform targetPosition;
    public float smoothingSpeed = 2.0f;
    public CameraController controller;
    public UIController uIController;
    private bool hasTriggeredDialogue = false;
    // Call this method to start moving the camera
    private void Update()
    {
        Vector2 targetPos = targetPosition.position;
        Vector2 thisPos = this.transform.position;
        if (Vector2.Distance(thisPos, targetPos) > 0.01f) { MoveCameraToTarget(); }

        if (Vector2.Distance(thisPos, targetPos) <= 0.01f && !hasTriggeredDialogue) {
            hasTriggeredDialogue = true;
            controller.ReachedPoint();
        }



    }

    private void MoveCameraToTarget()
    {
        if (targetPosition != null)
        {
            // Calculate the next position
            Vector2 targetPos = targetPosition.position;
            Vector2 smoothedPosition = Vector2.Lerp(transform.position, targetPos, smoothingSpeed * Time.deltaTime);

            // Move the camera to the smoothed position
            transform.position = smoothedPosition;
        }
    }

    public void ResetBool() 
    {
        hasTriggeredDialogue = false;
    }
}