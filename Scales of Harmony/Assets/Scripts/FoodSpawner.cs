using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public float spawnDelay = 2.0f;
    public Vector2 spawnAreaSize = new Vector2(10, 10);

    void Start()
    {
        InvokeRepeating("SpawnFood", spawnDelay, spawnDelay);
    }

    void SpawnFood()
    {
        Vector2 spawnPosition = new Vector2(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2)
        );
        Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
    }

}
