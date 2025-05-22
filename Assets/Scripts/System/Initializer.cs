using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // 需要使用協程

public class Initializer : MonoBehaviour
{
    [Header("第一個要載入的遊戲場景")]
    public string firstSceneToLoad = "MainMenuScene"; // 請確保這個名稱與你的主菜單場景文件名一致

    [Header("需要全局初始化的管理器 Prefabs")]
    public GameObject gameStateManagerPrefab;
    public GameObject gameEventManagerPrefab;
    // (可選) 如果你的 UIManager 和 PickUpManager 也是全局且跨場景的，也將它們的 Prefab 加到這裡
    // public GameObject uiManagerPrefab;
    // public GameObject pickUpManagerPrefab;
    // (以此類推，添加所有需要全局初始化的管理器 Prefab)


    IEnumerator Start()
    {
        // --- 步驟 1: 實例化所有全局管理器 Prefabs (如果它們還不存在) ---
        // 這些管理器 Prefab 本身應該有 Singleton 邏輯，所以我們只在 instance 為 null 時創建
        // 它們的 Awake 方法會處理 DontDestroyOnLoad

        if (GameStateManager.instance == null && gameStateManagerPrefab != null)
        {
            Instantiate(gameStateManagerPrefab);
            Debug.Log("Initializer: GameStateManager 實例化完成。");
        }

        if (GameEventManager.instance == null && gameEventManagerPrefab != null)
        {
            Instantiate(gameEventManagerPrefab);
            Debug.Log("Initializer: GameEventManager 實例化完成。");
        }

        // (可選) 為 UIManager 和 PickUpManager 做同樣的檢查和實例化
        // if (UIManager.instance == null && uiManagerPrefab != null)
        // {
        //     Instantiate(uiManagerPrefab);
        //     Debug.Log("Initializer: UIManager 實例化完成。");
        // }
        // if (PickUpManager.instance == null && pickUpManagerPrefab != null)
        // {
        //     Instantiate(pickUpManagerPrefab);
        //     Debug.Log("Initializer: PickUpManager 實例化完成。");
        // }


        // --- 步驟 2: 等待一幀，確保所有剛實例化的管理器的 Awake() 方法都已執行 ---
        // 這樣它們的 instance 變數就會被正確設定，並且 DontDestroyOnLoad 也會生效。
        yield return null;


        // --- 步驟 3: 設定初始遊戲狀態 (由 GameStateManager 自己決定或在這裡統一設置) ---
        // 在上一個回覆中，我們建議由 Initializer 來設定初始狀態，以保持 GameStateManager 的 Start 更通用。
        if (GameStateManager.instance != null)
        {
            if (firstSceneToLoad.Equals("MainMenuScene", System.StringComparison.OrdinalIgnoreCase)) // 不區分大小寫比較
            {
                GameStateManager.instance.ChangeState(GameState.MainMenu); // 第二個參數 isInitialSetup = true
            }
            // 你可以為其他可能的啟動場景添加 else if 判斷
            // 例如，如果直接跳過主選單進入遊戲 (測試用):
            // else if (firstSceneToLoad.Equals("GameScene", System.StringComparison.OrdinalIgnoreCase))
            // {
            //     GameStateManager.instance.ChangeState(GameState.Gameplay, true);
            // }
            else
            {
                // 預設情況，或者如果 firstSceneToLoad 不是主選單也不是已知遊戲場景
                Debug.LogWarning($"Initializer: 未知的 firstSceneToLoad '{firstSceneToLoad}', 將默認設置遊戲狀態。考慮添加判斷。");
                // 你可以根據需求決定這裡的默認行為，例如，總是設置為 MainMenu，或如果不是 MainMenu 就設為 Gameplay
                GameStateManager.instance.ChangeState(GameState.MainMenu); // 先默認為 MainMenu
            }
        }
        else
        {
            Debug.LogError("Initializer: GameStateManager 未能在實例化後被正確引用！無法設定初始狀態。");
        }


        // --- 步驟 4: 載入第一個可見的遊戲場景 ---
        Debug.Log($"Initializer: 載入第一個場景: {firstSceneToLoad}");
        SceneManager.LoadScene(firstSceneToLoad);
    }
}