using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public EnemyData[] enemyTypes;
    public float spawnInterval = 5f;
    private float timer = 0f;
    private Camera mainCamera;
    public static EnemySpawnManager Instance { get; private set; }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        mainCamera = Camera.main;
    }



    public void SpawnEnemy()
    {
        Vector2 spawnPosition = GetBorderSpawnPosition();
        EnemyData selectedEnemy = enemyTypes[Random.Range(0, enemyTypes.Length)];
        GameObject enemy = Instantiate(selectedEnemy.enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.GetComponent<EnemyAI>().enemyData = selectedEnemy;
    }

    Vector2 GetBorderSpawnPosition()
    {
        // Calculate a random position at the border of the screen
        Vector2 spawnPoint = mainCamera.ViewportToWorldPoint(new Vector3(Random.value, Random.value, mainCamera.nearClipPlane));
        return new Vector2(spawnPoint.x, spawnPoint.y);
    }
}