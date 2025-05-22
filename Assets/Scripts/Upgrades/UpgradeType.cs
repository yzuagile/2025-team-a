// Scripts/Upgrades/UpgradeType.cs (或類似路徑)
public enum UpgradeType
{
    // 玩家基礎屬性
    IncreaseMaxHealth,
    IncreaseMovementSpeed,
    IncreasePickupRadius,
    IncreaseExperienceGain, // 增加經驗獲取百分比
    IncreaseArmor, // 增加護甲/減傷

    // 武器屬性 (可以更細分，例如針對特定武器)
    IncreaseAttackDamage,
    IncreaseProjectileSpeed,
    DecreaseAttackInterval, // 增加攻擊速度 (減少攻擊間隔)
    IncreaseProjectilesPerShot,
    IncreaseAttackRange,    // 增加武器索敵/有效範圍
    IncreaseProjectilePierce, // 增加彈幕穿透次數
    IncreaseCriticalHitChance,
    IncreaseCriticalHitDamage,
    IncreaseAreaOfEffect, // 增加效果範圍 (例如某些範圍武器)

    // 其他特殊效果
    GainNewWeapon, // 獲取新武器
    EvolveWeapon, // 進化武器
    SpecialAbility_Dash,
    SpecialAbility_Shield,

    // (更多你想要的類型...)
    None // 代表無或未定義
}