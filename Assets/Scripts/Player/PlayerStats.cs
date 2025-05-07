using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovements))]
public class PlayerStats : MonoBehaviour
{
    [Header("Player Level")]
    public int currentLevel = 1;
    public int currentExp = 1;
    public int expToNextLevel = 100;
    
    [Header("Player Health")]
    public float currentHealth;
    public float maxHealth = 100f;

    [Header("Player Attack")] 
    public float baseAttackDamage = 10f;
    public float baseProjectileSpeed = 8f;
    public float attackInterval = 1.0f; // 每次攻擊的間隔時間 (秒)
    public int projectilesPerShot = 1; // 一次發射多少個彈幕
    public float attackRange = 10f; // 武器索敵範圍
    
    [Header("Player Movement")]
    public float baseMoveSpeed = 5f;
    
    [Header("Player Pickup")]
    public float pickupRadius = 1.5f;
    
    private PlayerMovements playerMovements;

    void Awake()
    {
        currentHealth = maxHealth;
        playerMovements = GetComponent<PlayerMovements>();
        if (playerMovements != null)
        {
            playerMovements.movementSpeed = baseMoveSpeed;
        }
        else
        {
            Debug.LogError("PlayerStats: 找不到 PlayerMovements 組件！");
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetPlayerAttackDamage() { return baseAttackDamage; }
    public float GetPlayerProjectileSpeed() { return baseProjectileSpeed; }
    public float GetPlayerAttackInterval() { return attackInterval; }
    public int GetPlayerProjectilesPerShot() { return projectilesPerShot; }

    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0)
            return;
        currentHealth -= damage;
        Debug.Log($"PlayerStats: 玩家受到 {damage} 點傷害, 剩餘 HP: {currentHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("PlayerStats: 玩家死亡！");
        GetComponent<PlayerMovements>().enabled = false;
        if (GetComponent<PlayerAttackController>() != null) {
            GetComponent<PlayerAttackController>().enabled = false;
        }
    }
}
