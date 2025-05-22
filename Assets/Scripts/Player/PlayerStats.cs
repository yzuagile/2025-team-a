using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(PlayerMovements))]
public class PlayerStats : MonoBehaviour
{
    [Header("Player Level")]
    public int currentLevel = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;
    public float levelExpMultiplier = 1.5f;

    [Header("Player Health")]
    public float currentHealth;
    public float maxHealth = 100f;

    private Animator animator;
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

    [Header("Upgrade Option")]
    public List<UpgradeOptionData> availableUpgrades; // 所有可用的升級選項 Data Assets
    public int upgradesToShowPerLevel = 3; // 每次升級顯示幾個選項

    private PlayerMovements playerMovements;

    private UIManager uiManager;

    public GameObject gameOverPanel; // Assign in Inspector

    public GameObject Levelup;
    public Transform PlayerPosition;

    void Awake()
    {
        animator = GetComponent<Animator>();
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

        currentHealth = maxHealth;
    }

    void InitializeUI()
    {
        uiManager = UIManager.instance;
        if (uiManager == null)
        {
            Debug.LogError("PlayerStats: 找不到 UIManager 實例！UI 可能無法更新。");
            return;
        }

        uiManager.UpdateHealthUI(currentHealth, maxHealth);
        uiManager.UpdateExperienceUI(currentExp, expToNextLevel);
        uiManager.UpdateLevelText(currentLevel);
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeUI();
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

        // --- 更新 UI ---
        uiManager?.UpdateHealthUI(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        animator.SetBool("IsDie", true);
        Debug.Log("PlayerStats: 玩家死亡！");
        GetComponent<PlayerMovements>().enabled = false;
        if (GetComponent<PlayerAttackController>() != null)
        {
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
            LevelUp();
            leveledUp = true;
        }

        // --- 更新 UI ---
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

        //  更新UI
        uiManager?.UpdateExperienceUI(currentExp, expToNextLevel);
        uiManager?.UpdateLevelText(currentLevel);
        //  升級時血量回滿
        currentHealth = maxHealth;
        uiManager?.UpdateHealthUI(currentHealth, maxHealth);
        //  觸發升級選項 UI
        uiManager?.ShowUpgradePanel();

        List<UpgradeOptionData> chosenOptions = ChooseUpgradeOptions();
        uiManager.ShowUpgradePanel(chosenOptions);
    }

    List<UpgradeOptionData> ChooseUpgradeOptions()
    {
        List<UpgradeOptionData> options = new List<UpgradeOptionData>();
        if (availableUpgrades == null || availableUpgrades.Count == 0)
        {
            Debug.LogWarning("PlayerStats: 可用升級列表 (availableUpgrades) 為空！");
            return options; // 返回空列表
        }

        // 創建一個可用升級的臨時副本，避免直接修改原始列表
        List<UpgradeOptionData> possibleOptions = new List<UpgradeOptionData>(availableUpgrades);
        // (可選) 在這裡可以加入篩選邏輯，例如移除玩家已經達到上限的升級

        int optionsToPick = Mathf.Min(upgradesToShowPerLevel, possibleOptions.Count);

        for (int i = 0; i < optionsToPick; i++)
        {
            if (possibleOptions.Count == 0) break; // 如果沒有更多選項可以選了

            // --- 根據權重隨機選擇 ---
            int totalWeight = 0;
            foreach (var option in possibleOptions)
            {
                totalWeight += option.selectionWeight;
            }

            if (totalWeight <= 0) // 如果所有剩餘選項權重都為0 (不應發生除非數據配置錯誤)
            {
                // 備用：純隨機選一個
                int randIndexFallback = Random.Range(0, possibleOptions.Count);
                UpgradeOptionData chosen = possibleOptions[randIndexFallback];
                options.Add(chosen);
                possibleOptions.RemoveAt(randIndexFallback); // 從臨時列表中移除，避免重複選擇
                continue;
            }

            int randomNumber = Random.Range(0, totalWeight);
            UpgradeOptionData selectedOption = null;
            int processedWeight = 0;

            foreach (var option in possibleOptions)
            {
                processedWeight += option.selectionWeight;
                if (randomNumber < processedWeight)
                {
                    selectedOption = option;
                    break;
                }
            }
            // -----------------------

            if (selectedOption != null)
            {
                options.Add(selectedOption);
                possibleOptions.Remove(selectedOption); // 從臨時列表中移除，避免重複選擇
            }
            else if (possibleOptions.Count > 0) // 如果權重選擇失敗 (理論上不應發生)，就隨便選一個
            {
                Debug.LogWarning("PlayerStats: 權重選擇升級失敗，進行隨機備選。");
                int randIndexFallback = Random.Range(0, possibleOptions.Count);
                UpgradeOptionData chosen = possibleOptions[randIndexFallback];
                options.Add(chosen);
                possibleOptions.RemoveAt(randIndexFallback);
            }
        }
        return options;
    }

    // 由 UpgradeButtonUI 點擊後呼叫 (或由 UIManager 中轉呼叫)
    public void ApplyUpgrade(UpgradeOptionData chosenUpgrade)
    {
        if (chosenUpgrade == null)
        {
            Debug.LogError("PlayerStats: 嘗試應用的升級數據為 null！");
            return;
        }

        Debug.Log($"應用升級: {chosenUpgrade.upgradeName}");

        switch (chosenUpgrade.upgradeType)
        {
            case UpgradeType.IncreaseMaxHealth:
                maxHealth += chosenUpgrade.value1;
                currentHealth += chosenUpgrade.value1; // 同時增加當前血量 (或者只加最大，讓玩家找補血)
                InitializeUI(); // 更新UI
                break;
            case UpgradeType.IncreaseMovementSpeed:
                baseMoveSpeed += chosenUpgrade.value1;
                if (playerMovements != null) playerMovements.movementSpeed = baseMoveSpeed; // 直接更新移動組件的速度
                break;
            case UpgradeType.IncreasePickupRadius:
                pickupRadius += chosenUpgrade.value1;
                break;
            case UpgradeType.IncreaseAttackDamage:
                baseAttackDamage += chosenUpgrade.value1;
                break;
            case UpgradeType.IncreaseProjectileSpeed:
                baseProjectileSpeed += chosenUpgrade.value1;
                break;
            case UpgradeType.DecreaseAttackInterval:
                attackInterval -= chosenUpgrade.value1;
                if (attackInterval < 0.1f) attackInterval = 0.1f; // 設置一個最小攻擊間隔避免太快
                break;
            case UpgradeType.IncreaseProjectilesPerShot:
                projectilesPerShot += (int)chosenUpgrade.value1;
                break;
            case UpgradeType.IncreaseAttackRange:
                attackRange += chosenUpgrade.value1;
                break;
            default:
                Debug.LogWarning($"PlayerStats: 未處理的升級類型: {chosenUpgrade.upgradeType}");
                break;
        }
        // (可選) 播放一個成功的音效或小動畫
    }
}
