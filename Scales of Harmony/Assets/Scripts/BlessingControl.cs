using System.Collections.Generic;
using UnityEngine;

public class BlessingControl : MonoBehaviour
{
    public Blessing assignedBlessing;
    public Blessing blessing;
    public GameManager gameManager;
    public PowerManager powerManager;
    public List<Blessing> AllBlessings;
    public GameObject dragon;
    private float timer = 0f;

    public void Initialize(Blessing blessing)
    {
        assignedBlessing = blessing;
        gameManager = FindAnyObjectByType<GameManager>();
        powerManager = FindAnyObjectByType<PowerManager>();
        dragon = powerManager.dragon;
        AllBlessings = powerManager.AllBlessings;
    // Initialize other necessary properties or states
}

    void Update()
    {
        if (assignedBlessing == null) return;

        timer += Time.deltaTime;
        if (timer >= assignedBlessing.attackInterval)
        {
            ExecuteBlessingAction();
            timer = 0f;
        }
    }

    private void ExecuteBlessingAction()
    {
        // Perform the action specific to the assigned blessing
        // This might involve calling a method on the Blessing ScriptableObject
        // or implementing the logic directly here, depending on your design
        switch (assignedBlessing.blessingNo)
        {
            case 0:
                // Moss : Magic Ball
                Shooting(0);
                break;
            case 1:
                // Moss: Flame Breath
                break;
            case 2:
                // Moss: Dragon Nova
                break;
            default:
                // code block
                break;
        }
    }


    public void Shooting(int BlessingNo)
    {
        Vector3 cursorScreenPosition = Input.mousePosition;
        Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(cursorScreenPosition);
        cursorWorldPosition.z = 0; // Assuming a 2D game
        Blessing currentBlessing = AllBlessings[BlessingNo];
        GameObject projectileObject = Instantiate(currentBlessing.projectileData.projectilePrefab, dragon.transform.position, Quaternion.identity);
        projectileObject.GetComponent<Projectile>().Initialize(cursorWorldPosition, currentBlessing.projectileData); // use the scriptable object data
    }

}