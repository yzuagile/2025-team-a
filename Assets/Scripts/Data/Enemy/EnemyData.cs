using UnityEngine;

// [CreateAssetMenu] 屬性讓你可以從 Unity Editor 的 Assets/Create 選單中直接創建這種數據 Asset
[CreateAssetMenu(fileName = "New EnemyData", menuName = "Enemy Data", order = 51)]
public class EnemyData : ScriptableObject // 注意：繼承自 ScriptableObject 而不是 MonoBehaviour
{
    public GameObject prefab;
    
    [Header("基本識別")]
    public string enemyName = "New Enemy";
    // (可選) 如果你想根據類型做特殊判斷，可以加個枚舉
    // public EnemyType enemyType;

    [Header("核心屬性")]
    public float maxHealth = 10f;
    public float movementSpeed = 2f;
    public float contactDamage = 5f;

    [Header("掉落")]
    public int experienceDropped = 10;

    // [Header("視覺與預製件")]
    // public Sprite displaySprite; // (可選) 可以連 Sprite 也定義在這
    // public RuntimeAnimatorController animatorController; // (可選) 動畫控制器
    // public GameObject prefab; // (重要！考慮將 Prefab 也放入 Data 中管理)

    // (未來還可以加入：攻擊力、攻擊範圍、特殊技能、抗性等等...)
}

// (可選) 定義 EnemyType 枚舉
// public enum EnemyType
// {
//     Slime,
//     Bat,
//     Goblin
//     // ...
// }