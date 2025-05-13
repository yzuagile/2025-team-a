using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerAttackController : MonoBehaviour
{
    private PlayerStats playerStats;
    
    public GameObject projectilePrefab;
    
    public float attackRange = 10f;
    public float attackTimer = 0f;
    public LayerMask enemyLayer;
    
    void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerWeaponController: 找不到 PlayerStats 組件！");
            this.enabled = false;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStats == null)
            return;
        attackTimer += Time.deltaTime;
        
        float currentAttackInterval = playerStats.GetPlayerAttackInterval(); // 假設有方法獲取
        float currentProjectileSpeed = playerStats.GetPlayerProjectileSpeed();
        float currentDamage = playerStats.GetPlayerAttackDamage(); // 或 GetCalculatedDamage()
        int currentProjectilesPerShot = playerStats.GetPlayerProjectilesPerShot();

        if (attackTimer >= currentAttackInterval)
        {
            attackTimer = 0f;
            
            Transform nearestEnemy = FindNearestEnemyOptimized(attackRange);
            if (nearestEnemy != null)
            {
                for (int i = 0; i < playerStats.GetPlayerProjectilesPerShot(); i++)
                {
                    GameObject projectileObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                    Projectile projectileScript = projectileObj.GetComponent<Projectile>();

                    if (projectileScript != null)
                    {
                        projectileScript.Setup(nearestEnemy, currentProjectileSpeed, currentDamage);
                    }
                    else
                    {
                        Debug.LogError("PlayerAttackController: 生成的物件上沒有 Projectile 腳本！檢查 Prefab 配置。");
                        Destroy(projectileObj);
                    }
                }
            }
        }
    }
    
    Transform FindNearestEnemyOptimized(float searchRadius)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, searchRadius, enemyLayer);
        Transform nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            EnemyStats enemy = hit.GetComponent<EnemyStats>();
            if (enemy != null && enemy.gameObject.activeInHierarchy)
            {
                float distance = Vector3.Distance(hit.transform.position, transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = hit.transform; // 或 enemy.transform
                }
            }
        }
        return nearestEnemy;
    }
}
