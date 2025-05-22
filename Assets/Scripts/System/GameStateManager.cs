using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 用於場景切換

public enum GameState
{
    MainMenu,
    Gameplay,
    Paused,
    LevelUp, // 獎勵選擇
    GameOver
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    public GameState CurrentState { get; private set; } // 私有 set，只能透過方法改變

    // 可以定義事件來通知狀態改變
    public delegate void GameStateChangedAction(GameState newState, GameState prevState);
    public static event GameStateChangedAction OnGameStateChanged;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState && newState != GameState.LevelUp)
            return; // 允許重複進入 LevelUp (例如連續升級的情況)

        GameState prevState = CurrentState;
        CurrentState = newState;

        Debug.Log($"GameState changed from {prevState} to {CurrentState}");
        OnGameStateChanged?.Invoke(CurrentState, prevState);

        // 根據新狀態執行特定邏輯
        switch (CurrentState)
        {
            case GameState.MainMenu:
                // Time.timeScale = 1f; // 主選單時時間正常
                // 載入主選單場景
                // SceneManager.LoadScene("MainMenuScene");
                break;
            case GameState.Gameplay:
                Time.timeScale = 1f; // 遊戲進行時時間正常
                if (UIManager.instance != null && prevState == GameState.LevelUp) {
                    UIManager.instance.HideUpgradePanel(); // 確保從升級狀態切回時面板關閉
                }// (如果從其他狀態切回 Gameplay，確保玩家可以操作等)
                // EventManager.TriggerEvent(GameEvent.GameResumed);
                break;
            case GameState.Paused:
                Time.timeScale = 0f; // 遊戲暫停
                // 顯示暫停選單 UI
                // EventManager.TriggerEvent(GameEvent.GamePaused);
                break;
            case GameState.LevelUp:
                Time.timeScale = 0f; // 升級選擇時遊戲暫停 (UIManager 已處理)
                // UIManager 已通過 PlayerStats 的 LevelUp 呼叫來顯示面板
                break;
            case GameState.GameOver:
                Time.timeScale = 0f; // 遊戲結束時遊戲暫停 (也可以慢動作)
                // 顯示遊戲結束 UI
                // EventManager.TriggerEvent(GameEvent.GameOver);
                break;
        }
    }

    // 其他系統如何查詢狀態：
    // if (GameStateManager.instance.CurrentState == GameState.Gameplay) { ... }

    // 提供一些常用的狀態轉換方法 (範例)
    public void PauseGame()
    {
        if (CurrentState == GameState.Gameplay)
        {
            ChangeState(GameState.Paused);
        }
    }

    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
        {
            ChangeState(GameState.Gameplay);
        }
    }

    public void PlayerDied() // 由 PlayerStats.Die() 呼叫
    {
         ChangeState(GameState.GameOver);
    }

    public void StartLevelUpSequence() // 由 PlayerStats.LevelUp() 呼叫 (在顯示面板前)
    {
         ChangeState(GameState.LevelUp);
    }
     public void EndLevelUpSequence() // 由 UIManager 在關閉升級面板時呼叫
    {
         if(CurrentState == GameState.LevelUp)
            ChangeState(GameState.Gameplay);
    }
     
    // --- 由 PlayerStats.LevelUp() 呼叫 ---
    public void EnterLevelUpState()
    {
        if (CurrentState == GameState.Gameplay || CurrentState == GameState.LevelUp) // 允許從 Gameplay 或連續升級時進入
        {
            ChangeState(GameState.LevelUp);
        }
        else
        {
            Debug.LogWarning($"GameStateManager: 無法從 {CurrentState} 狀態進入 LevelUp 狀態。");
        }
    }

    // --- 由 UIManager (或 UpgradeButtonUI) 在玩家做出選擇後呼叫 ---
    public void ExitLevelUpState()
    {
        if (CurrentState == GameState.LevelUp)
        {
            ChangeState(GameState.Gameplay);
        }
        else
        {
            Debug.LogWarning($"GameStateManager: 當前不處於 LevelUp 狀態，無法退出。目前狀態: {CurrentState}");
        }
    }
}
