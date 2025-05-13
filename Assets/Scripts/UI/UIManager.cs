using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance; // 簡化版靜態變數
    
    [Header("狀態顯示 UI 元素")]
    public Slider healthSlider;
    public Slider expSlider;
    public TMP_Text levelText;
    
    [Header("升級選項面板")]
    public GameObject upgradePanel; // 整個升級面板的父物件 (目前先放著)

    [Header("暫停畫面")]
    public GameObject pausePanel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("UIManager: 場景中發現多個 UIManager 實例！", this);
            Destroy(gameObject);
        }

        if (upgradePanel != null && upgradePanel.activeInHierarchy)
        {
            upgradePanel.SetActive(false);
        }

        // 保險：確保時間縮放是正常的
        Time.timeScale = 1f;
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
            HideUpgradePanel();
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
    
    public void HideUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
            Time.timeScale = 1f; // 關閉面板時恢復遊戲
            Debug.Log("UIManager: 隱藏升級面板，遊戲已恢復。");
        }
    }

    public void TogglePausePanel()
    {
        if (pausePanel != null)
        {
            bool isActive = pausePanel.activeSelf;
            pausePanel.SetActive(!isActive);
            Time.timeScale = isActive ? 1f : 0f;
            Debug.Log(isActive ? "繼續遊戲" : "遊戲暫停");
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
}
