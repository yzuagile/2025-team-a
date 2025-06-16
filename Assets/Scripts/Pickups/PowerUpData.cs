// Scripts/Pickups/PowerUpData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "New PowerUpData", menuName = "Pickups/PowerUp Data", order = 53)]
public class PowerUpData : ScriptableObject
{
    [Header("基本資訊")]
    public PowerUpType type = PowerUpType.None;
    public string powerUpName = "新道具";
    public Sprite icon; // (可選) 道具在地面上或UI上的圖示

    [Header("效果參數")]
    public float duration = 5f;  // Buff 的持續時間 (秒)
    public float value1 = 0f;    // 效果數值 (例如：增加的速度值、增加的傷害值)
    public float value2 = 0f;    // (可選) 備用數值

    [Header("視覺 Prefab")]
    public GameObject pickupPrefab; // 對應的掉落物預製件
}