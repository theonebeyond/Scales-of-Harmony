using UnityEngine;

public class SoulOrb : MonoBehaviour
{
    public float attractionRadius = 0.1f; // Radius within which the orb is attracted to the player
    public float attractionStrength = 1.0f; // How strong the attraction is
    private GameObject player; // Reference to the player GameObject

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Dragon"); // Find the player by tag
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < attractionRadius)
            {
                // Calculate the attraction force based on distance
                float forceStrength = Mathf.Lerp(attractionStrength, 0, distance / attractionRadius);

                // Move the orb towards the player
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, forceStrength * Time.deltaTime);
            }
        }
    }
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
