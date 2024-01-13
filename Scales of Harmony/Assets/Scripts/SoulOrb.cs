using UnityEngine;

public class SoulOrb : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Dragon"))
        {
            // Gain EXP
            EXPManager eXPManager = FindAnyObjectByType<EXPManager>();
            eXPManager.GainExp(1); // Adjust the amount as needed

            Destroy(gameObject);
        }
    }
}
