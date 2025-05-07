using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyData> enemiesToSpawn;
    
    public float spawnMinX = -24;
    public float spawnMaxX = 24;
    public float spawnMinY = -24;
    public float spawnMaxY = 24;

    public int maxEnemies = 20;
    
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    
    private float spawnInterval = 1f;
    private float spawnTimer = 0f;

    void Awake()
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        handleSpawnTimer();
    }

    void FixedUpdate()
    {
        
    }

    void handleSpawnTimer()
    {
        CleanUpSpawnedEnemiesList();
        spawnTimer += Time.deltaTime;
        
        if (spawnTimer >= spawnInterval &&
            spawnedEnemies.Count < maxEnemies && 
            enemiesToSpawn.Count > 0)
        {
            spawnTimer -= spawnInterval;
            spawnEnemy();
        }
    }
    
    void spawnEnemy()
    {
        int randomIndex = Random.Range(0, enemiesToSpawn.Count);
        EnemyData selectedEnemyData = enemiesToSpawn[randomIndex]; 
        if (selectedEnemyData != null && selectedEnemyData.prefab != null)
        {
            float x = Random.Range(spawnMinX, spawnMaxX);
            float y = Random.Range(spawnMinY, spawnMaxY);
            Vector3 pos = new Vector3(x, y, 0);
            GameObject newEnemy =  Instantiate(selectedEnemyData.prefab, pos, Quaternion.identity);

            
            // --- 重要：確保新生成的敵人知道自己的 EnemyData (雖然 EnemyStats 會從 Prefab 獲取) ---
            // 通常 EnemyStats 已經在 Prefab 上配置好了，會自動在 Awake 中讀取正確的 EnemyData 引用
            // 但如果需要額外設置或驗證，可以在這裡做：
            EnemyStats stats = newEnemy.GetComponent<EnemyStats>();
            if (stats != null && stats.enemyData == null) // 以防 Prefab 上的引用意外丟失
            {
                stats.enemyData = selectedEnemyData;
                Debug.LogWarning($"為 {newEnemy.name} 強制設置了 EnemyData，請檢查 Prefab 配置。");
            }
            // -----------------------------------------------------------------------------
            
            spawnedEnemies.Add(newEnemy);
        }
        else
        {
            Debug.LogWarning($"EnemySpawner: 在索引 {randomIndex} 處選擇的 EnemyData 或其 Prefab 無效。");
        }
    }

    void CleanUpSpawnedEnemiesList()
    {
        spawnedEnemies.RemoveAll(item => item == null);
    }
}
