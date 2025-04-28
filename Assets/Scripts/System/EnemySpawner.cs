using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject greenSlimePrefab;
    
    public float spawnMinX = -25;
    public float spawnMaxX = 25;
    public float spawnMinY = -25;
    public float spawnMaxY = 25;
    
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
        
    }

    void FixedUpdate()
    {
        handleSpawnTimer();
    }

    void handleSpawnTimer()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer -= spawnInterval;
            spawnEnemy();
        }
    }
    
    void spawnEnemy()
    {
        float x = Random.Range(spawnMinX, spawnMaxX);
        float y = Random.Range(spawnMinY, spawnMaxY);
        Vector3 pos = new Vector3(x, y, 0);
        Instantiate(greenSlimePrefab, pos, Quaternion.identity);
    }
}
