using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ExperienceOrb : MonoBehaviour
{
    public int experienceValue = 1; // 默認值，會被生成時覆蓋
    public float moveSpeed = 6f;     // 朝向玩家時的移動速度
    public float detectionDelay = 0.2f; // 開始檢測玩家前的延遲 (防止剛生成就吸附)

    private Rigidbody2D rb;
    private PlayerStats playerStats;     // 玩家屬性組件引用
    private Transform playerTransform;   // 玩家位置引用
    private bool canMoveTowardsPlayer = false;
    private float delayTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform  = playerObj.transform;
            playerStats = playerObj.GetComponent<PlayerStats>();
        }
        else
        {
            Debug.LogError("ExperienceOrb: 找不到玩家！無法運作。");
            Destroy(gameObject);
        }
        
        delayTimer = detectionDelay; // 初始化延遲計時器
    }

    public void SetExperience(int value)
    {
        this.experienceValue = value;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null || playerStats == null)return;

        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
            return;
        }
        
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= playerStats.pickupRadius)
        {
            canMoveTowardsPlayer = true;
        }
        else
        {
            // 如果需要離開範圍停止追蹤，可以在這裡設置
            // canMoveTowardsPlayer = false;
            // 但典型吸血鬼倖存者是一旦吸附就追到底。
        }
    }
    
    void FixedUpdate()
    {
        if (canMoveTowardsPlayer && playerTransform == null)return;
        // 計算朝向玩家的方向
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        // 計算位移
        Vector2 displacement = direction * moveSpeed * Time.fixedDeltaTime;
        
        rb.MovePosition(rb.position + displacement);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")return;
        if (playerStats == null)return;
        playerStats.GainExperience(experienceValue);
        Destroy(gameObject);
    }
}
