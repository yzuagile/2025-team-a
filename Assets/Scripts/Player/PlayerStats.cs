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
    public float attackInterval = 1.0f;
    public int projectilesPerShot = 1;
    public float attackRange = 10f;

    [Header("Player Movement")]
    public float baseMoveSpeed = 5f;

    [Header("Player Pickup")]
    public float pickupRadius = 1.5f;

    private PlayerMovements playerMovements;
    private UIManager uiManager;

    public GameObject gameOverPanel; // Assign in Inspector

    void Awake()
    {
        // 初始化移動組件
        playerMovements = GetComponent<PlayerMovements>();
        if (playerMovements != null)
        {
            playerMovements.movementSpeed = baseMoveSpeed;
        }
        else
        {
            Debug.LogError("PlayerStats: 找不到 PlayerMovements 組件！");
        }

        // 設定初始血量
        currentHealth = maxHealth;
    }

    void Start()
    {
        // 嘗試取得 UIManager 實例
        uiManager = UIManager.instance;
        if (uiManager == null)
        {
            Debug.LogError("PlayerStats: Start 時找不到 UIManager 實例！");
        }
        else
        {
            InitializeUI();
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // 開始時關閉死亡面板
        }
    }

    void InitializeUI()
    {
        uiManager?.UpdateHealthUI(currentHealth, maxHealth);
        uiManager?.UpdateExperienceUI(currentExp, expToNextLevel);
        uiManager?.UpdateLevelText(currentLevel);
    }

    void Update()
    {
        // 可擴充 Update 行為
    }

    public float GetPlayerAttackDamage() => baseAttackDamage;
    public float GetPlayerProjectileSpeed() => baseProjectileSpeed;
    public float GetPlayerAttackInterval() => attackInterval;
    public int GetPlayerProjectilesPerShot() => projectilesPerShot;

    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        Debug.Log($"PlayerStats: 玩家受到 {damage} 點傷害, 剩餘 HP: {currentHealth}");

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

        PlayerAttackController attackController = GetComponent<PlayerAttackController>();
        if (attackController != null)
        {
            attackController.enabled = false;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void GainExperience(int amount)
    {
        currentExp += amount;
        Debug.Log($"PlayerStats: 獲得 {amount} 經驗, 當前經驗: {currentExp}/{expToNextLevel}");

        bool leveledUp = false;
        while (currentExp >= expToNextLevel)
        {
            LevelUp();
            leveledUp = true;
        }

        if (!leveledUp)
        {
            uiManager?.UpdateExperienceUI(currentExp, expToNextLevel);
        }
    }

    void LevelUp()
    {
        currentLevel++;
        currentExp -= expToNextLevel;
        expToNextLevel = Mathf.FloorToInt(expToNextLevel * levelExpMultiplier);

        Debug.Log($"PlayerStats: 升級到 Lv. {currentLevel}! 下一級需要 {expToNextLevel} 經驗。");

        currentHealth = maxHealth;

        // 更新 UI
        uiManager?.UpdateExperienceUI(currentExp, expToNextLevel);
        uiManager?.UpdateLevelText(currentLevel);
        uiManager?.UpdateHealthUI(currentHealth, maxHealth);
        uiManager?.ShowUpgradePanel();
    }
}
