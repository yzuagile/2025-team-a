using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyStats : MonoBehaviour
{
    [Header("ScriptableObject")] public EnemyData enemyData;
    
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentMovementSpeed;
    
    public GameObject experienceOrbPrefab;
    void Awake()
    {
        if (enemyData != null)
        {
            currentHealth = enemyData.maxHealth;
            currentMovementSpeed = enemyData.movementSpeed;
        }
        else
        {
            Debug.LogError($"EnemyData 未在 {gameObject.name} 上設定！");
            currentHealth = 1f;
        }

        if (experienceOrbPrefab == null)
        {
            Debug.LogError($"EnemyStates:　經驗球 Prefab 未在 {gameObject.name} 的 EnemyStats 上設定！將無法掉落經驗。");
        }
    }
    
    public float GetMaxHealth() { return enemyData != null ? enemyData.maxHealth : 1f; }
    public float GetBaseMovementSpeed() { return enemyData != null ? enemyData.movementSpeed : 0f; }
    public float GetContactDamage() { return enemyData != null ? enemyData.contactDamage : 0f; }
    public int GetExperienceDropped() { return enemyData != null ? enemyData.experienceDropped : 0; }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if (enemyData == null || currentHealth <= 0) return;
        
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            die();
        }
    }

    public void die()
    {
        Debug.Log($"EnemyStats: {enemyData.enemyName} ({gameObject.name}) 死亡！");
        if (experienceOrbPrefab != null && enemyData != null)
        {
            GameObject orbGO  = Instantiate(experienceOrbPrefab, transform.position, Quaternion.identity);
            ExperienceOrb orbScript = orbGO.GetComponent<ExperienceOrb>();
            if (orbScript != null)
                orbScript.SetExperience(GetExperienceDropped());
        }
        
        Destroy(gameObject);
    }
}
