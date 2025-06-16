// Scripts/Pickups/PowerUpPickup.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PowerUpPickup : MonoBehaviour
{
    public PowerUpData powerUpData; // 由生成它的地方設定

    void OnTriggerEnter2D(Collider2D other)
    {
        // 檢查是否碰到玩家
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                // 通知 PlayerStats 應用這個 PowerUp
                playerStats.ApplyPowerUp(powerUpData);
            }
            else
            {
                Debug.LogWarning("PowerUpPickup: 碰到的 Player 物件上沒有 PlayerStats！");
            }
            // 銷毀自身
            Destroy(gameObject);
        }
    }
}