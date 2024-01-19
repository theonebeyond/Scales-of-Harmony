using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public EnemyData[] enemyTypes;
    public BossData[] MiniBossTypes;
    public BossData FinalBossType;
    public GameObject bossLocation;
    public float spawnInterval = 5f;
    private Camera mainCamera;
    public static EnemySpawnManager Instance { get; private set; }

    /*
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
    }*/
    void Start()
    {
        mainCamera = Camera.main;
    }



    public void SpawnEnemy(int currentLevel)
    {
        Vector2 spawnPosition = GetBorderSpawnPosition();
        EnemyData selectedEnemy = enemyTypes[Random.Range(0, enemyTypes.Length)];
        GameObject enemy = Instantiate(selectedEnemy.enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.GetComponent<EnemyAI>().enemyData = selectedEnemy;
        enemy.GetComponent<EnemyAI>().currentlevel = currentLevel;
    }

    public void SpawnBoss(int currentLevel)
    {
        Vector2 spawnPosition = bossLocation.transform.position;
        if (currentLevel < 2) {
            BossData selectedBoss = MiniBossTypes[Random.Range(0, MiniBossTypes.Length)];
            GameObject Boss = Instantiate(selectedBoss.bossPrefab, spawnPosition, Quaternion.identity);
            Boss.GetComponent<BOSSAI>().bossData = selectedBoss;
        }
        if (currentLevel == 2)
        {
            BossData selectedBoss = FinalBossType;
            GameObject Boss = Instantiate(selectedBoss.bossPrefab, spawnPosition, Quaternion.identity);
            Boss.GetComponent<BOSSAI>().bossData = selectedBoss;
        }

    }
    Vector2 GetBorderSpawnPosition()
    {
        // Find the main camera every time to avoid null reference after scene changes
        Camera currentCamera = Camera.main;
        if (currentCamera == null)
        {
            Debug.LogError("Main camera not found");
            return Vector2.zero;
        }

        Vector2 spawnPoint = currentCamera.ViewportToWorldPoint(new Vector3(Random.value, Random.value, currentCamera.nearClipPlane));
        return new Vector2(spawnPoint.x, spawnPoint.y);
    }
}