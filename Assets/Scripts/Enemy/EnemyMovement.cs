using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(EnemyStats))]
public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private EnemyStats enemyStats;
    private Transform playerTransform;

    private float moveSpeed;
    private float contactDamageCooldown = 1.0f; // �C���y���@���ˮ`
    private float lastDamageTime = -1.0f;

    private Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyStats = GetComponent<EnemyStats>();
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if (enemyStats != null && enemyStats.enemyData != null)
        {
            moveSpeed = enemyStats.GetBaseMovementSpeed();
        }
        else
        {
            Debug.LogWarning($"EnemyMovement: EnemyStats 或 EnemyData 在 {gameObject.name} 上未正確設定，無法獲取速度。");
            moveSpeed = 0f; // 無法獲取速度則不移動
        }
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("EnemyMovement: 找不到 Tag 為 'Player' 的物件！敵人無法確定目標。");
            // 這裡可以考慮禁用移動邏輯，因為沒有目標
            this.enabled = false; // 禁用此腳本
        }
    }

    // Update is called once per frame
    private bool isDead = false;
    void Update()
    {
        //isDead = anim.GetBool("isDead");
    }

    void FixedUpdate()
    {
        if (playerTransform == null || moveSpeed <= 0f)
        {
            return;
        }
        if (!enemyStats.canMove || enemyStats.currentHealth <= 0)
        {
            rb.velocity = Vector2.zero; // 確保完全停止
            return;
        }
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

        Vector2 displacement = directionToPlayer * moveSpeed * Time.fixedDeltaTime;

        Vector2 targetPosition = rb.position + displacement;

        rb.MovePosition(targetPosition);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (enemyStats == null || !other.CompareTag("Player")) return;

        if (Time.time >= lastDamageTime + contactDamageCooldown)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                float damageToDeal = enemyStats.GetContactDamage();
                playerStats.TakeDamage(damageToDeal);
                lastDamageTime = Time.time;
            }
        }
    }
}
