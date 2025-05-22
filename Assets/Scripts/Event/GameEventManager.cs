using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement; // 需要 Scene

public class SceneChangeEventArgs : EventArgs
{
    public Scene PreviousScene { get; private set; }
    public Scene NewScene { get; private set; }
    public LoadSceneMode Mode { get; private set; } // 場景加載模式

    public SceneChangeEventArgs(Scene previousScene, Scene newScene, LoadSceneMode mode)
    {
        PreviousScene = previousScene;
        NewScene = newScene;
        Mode = mode;
    }
}
public class EnemyEventArgs : EventArgs // 繼承自 EventArgs 是 C# 事件的慣例
{
    public GameObject deceasedEnemy { get; private set; } // 死亡的敵人本身
    public Vector3 position { get; private set; }         // 死亡位置
    public EnemyData enemyData { get; private set; }      // 敵人的數據 (包含經驗值等)

    public EnemyEventArgs(GameObject enemy, Vector3 pos, EnemyData data)
    {
        deceasedEnemy = enemy;
        position = pos;
        enemyData = data;
    }
}
public enum HealthChangeType
{
    Damage,
    Heal
}
// 修改 PlayerHealthEventArgs.cs
public class PlayerHealthEventArgs : EventArgs
{
    public HealthChangeType type { get; private set; }
    public float absoluteAmount { get; private set; } // 總是用正數表示變化量
    public float currentHealth { get; private set; }
    public float maxHealth { get; private set; }
    public GameObject instigator { get; private set; }

    public PlayerHealthEventArgs(HealthChangeType type, float amount, float currentHP, float maxHP, GameObject instigator = null)
    {
        this.type = type;
        this.absoluteAmount = Mathf.Abs(amount); // 確保是絕對值
        this.currentHealth = currentHP;
        this.maxHealth = maxHP;
        this.instigator = instigator;
    }
}
public class GameEventManager : MonoBehaviour
{
    public static GameEventManager instance;
    // Example with enum
    public enum GameEvent
    {
        EnemyDied,
        PlayerLeveledUp,
        PlayerTookDamage,
        GameOver,
        GamePaused,
        GameResumed,
        UpgradeChosen
        // ...更多事件
    }
    
    
    // 定義一個 delegate 簽名
    // 聲明一個靜態事件
    
    // ---- 敵人死亡事件 ----
    public delegate void EnemyDiedAction(EnemyEventArgs args);
    public static event EnemyDiedAction OnEnemyDied;
    // ---- 玩家生命值變化事件 ----
    public delegate void PlayerHealthChangedAction(PlayerHealthEventArgs args);
    public static event PlayerHealthChangedAction OnPlayerHealthChanged;
    // ---- Scene切換事件 ----
    public delegate void SceneChangedAction(SceneChangeEventArgs args);
    public static event SceneChangedAction OnSceneChanged;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // eventDictionary = new Dictionary<GameEvent, UnityEvent>();
            DontDestroyOnLoad(gameObject); // 事件管理器通常也需要跨場景
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public static void TriggerEnemyDied(GameObject enemy, Vector3 pos, EnemyData data)
    {
        EnemyEventArgs args = new EnemyEventArgs(enemy, pos, data);
        OnEnemyDied?.Invoke(args); // "?." 確保事件有訂閱者才呼叫
        Debug.Log($"GameEvent: Enemy Died - {data?.enemyName} at {pos}");
    }
    
    // --- 觸發玩家生命值變化事件的方法 ---
    public static void TriggerPlayerHealthChanged(HealthChangeType type, float amount, float currentHP, float maxHP, GameObject instigator = null)
    {
        PlayerHealthEventArgs args = new PlayerHealthEventArgs(type, amount, currentHP, maxHP, instigator);
        OnPlayerHealthChanged?.Invoke(args);
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