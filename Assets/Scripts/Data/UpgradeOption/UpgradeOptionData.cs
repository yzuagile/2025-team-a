// Scripts/Upgrades/UpgradeOptionData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "New UpgradeOption", menuName = "Upgrades/Upgrade Option Data", order = 52)]
public class UpgradeOptionData : ScriptableObject
{
    [Header("基礎資訊")]
    public string upgradeName = "新升級"; // 顯示在按鈕上的名稱
    [TextArea(3, 5)] // 讓描述欄位在 Inspector 中可以多行輸入
    public string description = "升級效果描述。"; // 更詳細的描述
    public Sprite icon; // (可選) 升級選項的圖示

    [Header("升級效果")]
    public UpgradeType upgradeType = UpgradeType.None; // 使用上面的枚舉
    public float value1; // 主要的數值 (例如：傷害增加量、速度增加量)
    // public float value2; // (可選) 次要數值 (例如：持續時間、額外效果概率)
    // public GameObject associatedPrefab; // (可選) 如果是解鎖新武器/技能，可能需要 Prefab
    // public int requiredPlayerLevel; // (可選) 此升級選項出現所需的最低玩家等級

    [Header("權重 (用於隨機選取)")]
    [Range(1, 100)]
    public int selectionWeight = 10; // 越大越容易被選中

    // (可選) 特定於升級類型的附加數據
    // public WeaponData newWeaponToGrant; // 如果是 UpgradeType.GainNewWeapon
}