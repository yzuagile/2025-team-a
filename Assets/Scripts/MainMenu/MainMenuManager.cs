// Scripts/UI/MainMenuManager.cs
using UnityEngine;
using UnityEngine.SceneManagement; // 需要這個來載入場景

public class MainMenuManager : MonoBehaviour
{
    // (可選) 可以指定下一個場景的名稱，而不是硬編碼
    public string characterSelectSceneName = "CharacterSelectScene"; // 假設角色選擇是單獨場景
    public string gameSceneName = "GameScene"; // 遊戲主場景名稱

    // 當 "Start Game" 按鈕被點擊時呼叫
    public void OnStartGameClicked()
    {
        Debug.Log("Start Game 按鈕被點擊！");

        // --- 選項 A：直接進入遊戲 (如果沒有角色選擇) ---
        // SceneManager.LoadScene(gameSceneName);

        // --- 選項 B：進入角色選擇界面 ---
        if (!string.IsNullOrEmpty(characterSelectSceneName))
        {
            SceneManager.LoadScene(characterSelectSceneName);
        }
        else
        {
            Debug.LogWarning("MainMenuManager: 角色選擇場景名稱未設定，將直接嘗試進入遊戲場景。");
            SceneManager.LoadScene(gameSceneName); // 作為備選方案
        }
    }

    // 當 "Quit Game" 按鈕被點擊時呼叫
    public void OnQuitGameClicked()
    {
        Debug.Log("Quit Game 按鈕被點擊！");
        Application.Quit(); // 在編輯器中可能無效，但在建置後會關閉應用程式

#if UNITY_EDITOR
        // 如果是在 Unity 編輯器中運行，這行可以停止播放模式
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}