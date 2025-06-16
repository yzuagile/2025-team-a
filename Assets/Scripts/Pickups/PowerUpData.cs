// Scripts/Pickups/PowerUpData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "New PowerUpData", menuName = "Pickups/PowerUp Data", order = 53)]
public class PowerUpData : ScriptableObject
{
    [Header("")]
    public PowerUpType type = PowerUpType.None;
    public string powerUpName = "speed";
    public Sprite icon;

    [Header("")]
    public float duration = 5f;
    public float value1 = 0f;
    public float value2 = 0f;

    [Header("Prefab")]
    public GameObject pickupPrefab;
}