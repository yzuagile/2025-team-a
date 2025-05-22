using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyStats : MonoBehaviour
{
    [Header("ScriptableObject")] public EnemyData enemyData;
    
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentMovementSpeed;
    [HideInInspector] public bool canMove = true;

    public int projectilesPerShot = 1;
    public float attackInterval = 1.0f; // 每次攻擊的間隔時間 (秒)

    public GameObject experienceOrbPrefab;
    
    private Animator animator;
    public bool isPlayerInRange = false;

    void Awake()
    {
        if (enemyData != null)
        {
            currentHealth = enemyData.maxHealth;
            currentMovementSpeed = enemyData.movementSpeed;
        }
        else
        {
            Debug.LogError($"EnemyData 未在 {gameObject.name} 上設定！");
            currentHealth = 1f;
        }

        if (experienceOrbPrefab == null)
        {
            Debug.LogError($"EnemyStates:　經驗球 Prefab 未在 {gameObject.name} 的 EnemyStats 上設定！將無法掉落經驗。");
        }
    }
    
    public float GetMaxHealth() { return enemyData != null ? enemyData.maxHealth : 1f; }
    public float GetBaseMovementSpeed() { return enemyData != null ? enemyData.movementSpeed : 0f; }
    public float GetContactDamage() { return enemyData != null ? enemyData.contactDamage : 0f; }
    public int GetExperienceDropped() { return enemyData != null ? enemyData.experienceDropped : 0; }
    

    private GameObject[] skeleton;
    // Start is called before the first frame update
    void Start()
    {
        skeleton = GameObject.FindGameObjectsWithTag("skeleton");
        animator=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"{gameObject.name} CurrentHealth: {currentHealth}");
        //anim.SetFloat("EnemyCurrentHealth", currentHealth);
        if (animator != null)
        {
            // 只有 skeleton 才會播放攻擊動畫
            if (CompareTag("skeleton"))
            {
                animator.SetBool("isPlayerInRange", isPlayerInRange);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (enemyData == null || currentHealth <= 0) return;
        
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            die();
        }
    }

    public void die()
    {

        Debug.Log($"EnemyStats: {enemyData.enemyName} ({gameObject.name}) 死亡！");
        if (experienceOrbPrefab != null && enemyData != null)
        {
            GameObject orbGO  = Instantiate(experienceOrbPrefab, transform.position, Quaternion.identity);
            ExperienceOrb orbScript = orbGO.GetComponent<ExperienceOrb>();
            if (orbScript != null)
                orbScript.SetExperience(GetExperienceDropped());
        }
        if (CompareTag("skeleton"))
        {
            canMove = false;
            gameObject.tag = "Untagged";
            StartCoroutine(DieWithAnimation());
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator DieWithAnimation()
    {
        animator.SetBool("isDead", true);
        
        // 播放死亡動畫（假設動畫名稱是 "Die"）
        animator.Play("Death Skeleton with sword");
        
        // 等待動畫時間，這裡假設動畫長度是 1 秒
        yield return new WaitForSeconds(1f);

        Debug.Log($"{gameObject.name}死亡，銷毀GameObject");
        Destroy(gameObject);
    }
}
