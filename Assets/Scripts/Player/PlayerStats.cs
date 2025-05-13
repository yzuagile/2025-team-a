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
    public float levelExpMultiplier = 1.5f;
    
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
    private UIManager uiManager;

    void Awake()
    {
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
    
    void InitializeUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateHealthUI(currentHealth, maxHealth);
            uiManager.UpdateExperienceUI(currentExp, expToNextLevel);
            uiManager.UpdateLevelText(currentLevel);
        }
        else
        {
            Debug.LogError("PlayerStats: Awake 時 UIManager 未就緒！");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManager.instance;
        if (uiManager == null)
        {
            Debug.LogError("PlayerStats: 找不到 UIManager 實例！UI 可能無法更新。");
        }
        else
        {
            currentHealth = maxHealth;
            uiManager?.UpdateHealthUI(currentHealth, maxHealth);
            uiManager?.UpdateExperienceUI(currentExp, expToNextLevel);
            uiManager?.UpdateLevelText(currentLevel);
            InitializeUI();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10f); // 按空白鍵 -10 HP
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            currentHealth += 10f; // 回 10 血
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            uiManager?.UpdateHealthUI(currentHealth, maxHealth);
        }
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
        
        // --- 更新 UI ---
        uiManager?.UpdateHealthUI(currentHealth, maxHealth);
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

    public void GainExperience(int amount)
    {
        currentExp += amount;
        Debug.Log($"PlayerStats: 獲得 {amount} 經驗, 當前經驗: {currentExp}/{expToNextLevel}");
        bool leveledUp = false;
        while (currentExp >= expToNextLevel)
        {
            LuvelUp();
            leveledUp = true;
        }
        
        // --- 更新 UI ---
        if (!leveledUp)
        {
            uiManager?.UpdateExperienceUI(currentExp, expToNextLevel);
        }
    }

    void LuvelUp()
    {
        currentLevel++;
        currentExp -= expToNextLevel;
        expToNextLevel = Mathf.FloorToInt(expToNextLevel * levelExpMultiplier);
        
        Debug.Log($"PlayerStats: 升級到 Lv. {currentLevel}! 下一級需要 {expToNextLevel} 經驗。");
        
        //  更新UI
        uiManager?.UpdateExperienceUI(currentExp, expToNextLevel);
        uiManager?.UpdateLevelText(currentLevel);
        //  升級時血量回滿
        currentHealth = maxHealth;
        uiManager?.UpdateHealthUI(currentHealth, maxHealth);
        //  觸發升級選項 UI
        uiManager?.ShowUpgradePanel();
    }
}
