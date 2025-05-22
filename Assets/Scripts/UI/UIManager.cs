using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("狀態顯示 UI 元素")]
    public Slider healthSlider;
    public Slider expSlider;
    public TMP_Text levelText;

    [Header("升級選項面板")]
    public GameObject upgradePanel;
    public List<UpgradeButtonUI> upgradeButtons;

    private PlayerStats playerStats;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            upgradePanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("UIManager: 場景中發現多個 UIManager 實例！", this);
            Destroy(gameObject);
        }
    }
    
    public void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        if (healthSlider != null)
        {
            if (maxHealth <= 0) maxHealth = 1f; // 避免除以零
            healthSlider.maxValue = maxHealth;
            healthSlider.value = Mathf.Clamp(currentHealth, 0, maxHealth); // 確保值在範圍內
        }
        else
        {
            Debug.LogWarning("UIManager: HP Slider 未設定！");
        }
    }
    
    public void UpdateExperienceUI(int currentExp, int expToNextLevel)
    {
        if (expSlider != null)
        {
            if (expToNextLevel <= 0) expToNextLevel = 1; // 避免除以零
            expSlider.maxValue = expToNextLevel;
            expSlider.value = Mathf.Clamp(currentExp, 0, expToNextLevel);
        }
        else
        {
            Debug.LogWarning("UIManager: EXP Slider 未設定！");
        }
    }
    
    public void UpdateLevelText(int level)
    {
        if (levelText != null)
        {
            levelText.SetText($"Lv. {level}"); // 使用 SetText for TMP
        }
        else
        {
            Debug.LogWarning("UIManager: Level Text 未設定！");
        }
    }
    
    public void ShowUpgradePanel(/* 可能需要傳入升級選項數據 List<UpgradeOptionData> options */)
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(true);
            Time.timeScale = 0f; // 顯示面板時暫停遊戲
            // (在這裡根據 options 數據填充選項按鈕...)
            Debug.Log("顯示升級面板，遊戲已暫停。");
        }
        else
        {
            Debug.LogWarning("UIManager: Upgrade Panel 未設定！無法顯示。");
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>(); // 同樣，實際項目中可優化
        if(playerStats == null)
        {
            Debug.LogError("UIManager: 找不到 PlayerStats！部分功能可能異常。");
        }
        upgradePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 由 PlayerStats 在升級時呼叫
    public void ShowUpgradePanel(List<UpgradeOptionData> optionsToShow)
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(true);
            Time.timeScale = 0f; // 顯示面板時暫停遊戲

            if (optionsToShow == null || optionsToShow.Count == 0)
            {
                Debug.LogWarning("UIManager: 沒有可顯示的升級選項！");
                // 可以考慮直接關閉面板或顯示 "無可用升級"
                HideUpgradePanel(); // 找不到選項就直接關閉
                return;
            }

            // 確保按鈕數量與選項數量匹配 (至少取較小值)
            int numOptionsToDisplay = Mathf.Min(optionsToShow.Count, upgradeButtons.Count);

            for (int i = 0; i < numOptionsToDisplay; i++)
            {
                if (i < upgradeButtons.Count && upgradeButtons[i] != null && i < optionsToShow.Count)
                {
                    upgradeButtons[i].gameObject.SetActive(true); // 確保按鈕是可見的
                    upgradeButtons[i].Setup(optionsToShow[i], playerStats); // 使用 PlayerStats 的引用
                }
                else
                {
                    Debug.LogError($"UIManager: 升級按鈕 {i} 或選項數據為 null。");
                    if (i < upgradeButtons.Count && upgradeButtons[i] != null)
                        upgradeButtons[i].gameObject.SetActive(false); // 隱藏無效按鈕
                }
            }

            // 如果選項比按鈕少，隱藏多餘的按鈕
            for (int i = numOptionsToDisplay; i < upgradeButtons.Count; i++)
            {
                if (upgradeButtons[i] != null)
                {
                    upgradeButtons[i].gameObject.SetActive(false);
                }
            }

            Debug.Log($"顯示升級面板，提供 {numOptionsToDisplay} 個選項，遊戲已暫停。");
        }
        else
        {
            Debug.LogWarning("UIManager: Upgrade Panel 未設定！無法顯示。");
        }
    }

    public void HideUpgradePanel() // 這個方法由 UpgradeButtonUI 呼叫
    {
        if (upgradePanel != null && upgradePanel.activeInHierarchy) // 確保只在面板可見時執行
        {
            upgradePanel.SetActive(false);
            // Debug.Log("UIManager: 隱藏升級面板。"); // 移到 GameStateManager

            // --- 通知 GameStateManager 退出升級狀態 ---
            if (GameStateManager.instance != null)
            {
                GameStateManager.instance.ExitLevelUpState(); // 這會處理 Time.timeScale = 1f;
            }
            else
            {
                Debug.LogError("UIManager: 找不到 GameStateManager 實例！遊戲可能不會恢復。");
                Time.timeScale = 1f; // 緊急恢復時間
            }
            // ----------------------------------------
        }
    }
    
    void OnEnable()
    {
        GameEventManager.OnPlayerHealthChanged += HandlePlayerHealthChanged;
    }

    void OnDisable()
    {
        GameEventManager.OnPlayerHealthChanged -= HandlePlayerHealthChanged;
    }

    private void HandlePlayerHealthChanged(PlayerHealthEventArgs args)
    {
        UpdateHealthUI(args.currentHealth, args.maxHealth);
        
        if (args.type == HealthChangeType.Damage)
        {
            Debug.Log($"UIManager: 玩家受到 {args.absoluteAmount} 傷害。");
            // 在這裡顯示傷害特效、聲音等
        }
        else if (args.type == HealthChangeType.Heal)
        {
            Debug.Log($"UIManager: 玩家恢復了 {args.absoluteAmount} 生命。");
            // 在這裡顯示治療特效、聲音等
        }
    }
}
