// Scripts/Managers/PickUpManager.cs
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    public static PickUpManager instance; // 如果其他地方也需要引用它

    public GameObject experienceOrbPrefab; // 引用經驗球 Prefab

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject); // 如果拾取物需要跨場景管理，則取消註解
        }
        else
        {
            Destroy(gameObject);
        }

        if (experienceOrbPrefab == null)
        {
            Debug.LogError("PickUpManager: Experience Orb Prefab 未設定！將無法生成經驗球。");
        }
    }

    // ---- 訂閱和取消訂閱事件 ----
    void OnEnable()
    {
        // 訂閱 GameEventManager 中的 OnEnemyDied 事件
        GameEventManager.OnEnemyDied += HandleEnemyDeath;
    }

    void OnDisable()
    {
        // 取消訂閱，防止內存洩漏或空引用
        GameEventManager.OnEnemyDied -= HandleEnemyDeath;
    }
    // ----------------------------

    // --- 事件處理方法 ---
    private void HandleEnemyDeath(EnemyEventArgs args)
    {
        if (args == null || args.enemyData == null || experienceOrbPrefab == null)
        {
            Debug.LogWarning("PickUpManager: HandleEnemyDeath 收到的參數不足或 Prefab 未設定。");
            return;
        }

        // 從 EnemyData 中獲取經驗值
        int expToDrop = args.enemyData.experienceDropped; // 假設 EnemyData 中有 experienceDropped

        if (expToDrop > 0) // 只有當有經驗值可掉落時才生成
        {
            // 在敵人死亡的位置生成經驗球
            GameObject orbGO = Instantiate(experienceOrbPrefab, args.position, Quaternion.identity);
            ExperienceOrb orbScript = orbGO.GetComponent<ExperienceOrb>();

            if (orbScript != null)
            {
                // 將這個敵人提供的經驗值傳遞給經驗球
                orbScript.SetExperience(expToDrop);
                // Debug.Log($"PickUpManager: 在 {args.position} 生成了代表 {expToDrop} 經驗的經驗球。");
            }
            else
            {
                Debug.LogError("PickUpManager: 生成的經驗球上沒有 ExperienceOrb 腳本！");
                Destroy(orbGO); // 銷毀無效物件
            }
        }
    }
    // -----------------
}