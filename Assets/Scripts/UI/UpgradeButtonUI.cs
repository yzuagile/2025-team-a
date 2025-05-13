// Scripts/UI/UpgradeButtonUI.cs
using UnityEngine;
using UnityEngine.UI; // 需要 Image
using TMPro; // 需要 TextMeshProUGUI

public class UpgradeButtonUI : MonoBehaviour
{
    public Image iconImage; // 按鈕上的圖示 (可選)
    public TMP_Text nameText;    // 顯示升級名稱
    public TMP_Text descriptionText; // 顯示升級描述

    private Button buttonComponent;
    private UpgradeOptionData currentOptionData;
    private UIManager uiManager; // 用來通知關閉面板

    void Awake()
    {
        buttonComponent = GetComponent<Button>();
        if (buttonComponent == null)
        {
            Debug.LogError("UpgradeButtonUI: 找不到 Button 組件！");
            return;
        }
        // 移除所有編輯器時期添加的監聽器 (以防萬一)
        buttonComponent.onClick.RemoveAllListeners();
        // 添加程式碼控制的監聽器
        buttonComponent.onClick.AddListener(OnButtonClicked);

        // 查找 UIManager (也可以在 Setup 時傳入)
        if (UIManager.instance != null)
        {
            uiManager = UIManager.instance;
        }
    }

    // 由 UIManager 呼叫來設定按鈕顯示的內容和點擊後執行的選項
    public void Setup(UpgradeOptionData optionData, PlayerStats playerStats)
    {
        currentOptionData = optionData;

        if (nameText != null)
            nameText.SetText(optionData.upgradeName);
        if (descriptionText != null)
            descriptionText.SetText(optionData.description);
        if (iconImage != null)
        {
            if (optionData.icon != null)
            {
                iconImage.sprite = optionData.icon;
                iconImage.enabled = true;
            }
            else
            {
                iconImage.enabled = false; // 如果沒有圖示則隱藏 Image 組件
            }
        }

        if (UIManager.instance != null)
        {
            uiManager = UIManager.instance;
        }
        // (重要) 將點擊事件與應用升級邏輯關聯起來
        // 這裡我們不在 AddListener 中直接應用，而是讓按鈕被點擊時，
        // 呼叫 UIManager 的方法，並把選擇的數據傳回去，由 UIManager 通知 PlayerStats。
        // 或者更直接：讓 PlayerStats 暴露應用方法，由這裡呼叫。
        // 為了演示，這裡我們先假設點擊後直接呼叫 PlayerStats 的 ApplyUpgrade 方法
    }

    void OnButtonClicked()
    {
        if (currentOptionData != null)
        {
            Debug.Log($"選擇了升級: {currentOptionData.upgradeName}");
            // 獲取 PlayerStats (這裡假設 PlayerStats 有一個靜態實例或通過 GameManager 獲取)
            PlayerStats playerStats = FindObjectOfType<PlayerStats>(); // 實際項目中應優化此查找
            if (playerStats != null)
            {
                playerStats.ApplyUpgrade(currentOptionData); // <--- PlayerStats 需要這個方法
            }

            // 通知 UIManager 關閉面板 (如果 UIManager 被找到)
            if (uiManager != null)
            {
                uiManager.HideUpgradePanel();
            }
            else
            {
                Debug.LogWarning("UpgradeButtonUI: 未找到 UIManager，無法自動關閉升級面板。");
                // 如果是這種情況，可以考慮讓升級面板自己監聽某個事件來關閉
            }
        }
    }
}